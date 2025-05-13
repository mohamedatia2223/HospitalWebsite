document.addEventListener("DOMContentLoaded", async function () {
    // Custom Country Dropdown Logic
    const countryDropdown = document.getElementById("custom-country-dropdown");
    const countryField = document.getElementById("countryField");
    const countryLabel = document.getElementById("countryLabel");
    const countrySelected = document.getElementById("countrySelected");
    const countryArrow = document.getElementById("countryArrow");
    const countryOptions = document.getElementById("countryOptions");
    const countryInput = document.getElementById("countryInput");
    // State Dropdown
    const stateDropdown = document.getElementById("custom-state-dropdown");
    const stateField = document.getElementById("stateField");
    const stateLabel = document.getElementById("stateLabel");
    const stateSelected = document.getElementById("stateSelected");
    const stateArrow = document.getElementById("stateArrow");
    const stateOptions = document.getElementById("stateOptions");
    const stateInput = document.getElementById("stateInput");
    let countryList = [];
    let isCountryOpen = false;
    let stateList = [];
    let isStateOpen = false;

    // Password match validation
    const passwordInput = document.getElementById("password");
    const passwordStrengthBox = document.getElementById("passwordStrengthBox");
    const passwordStrengthText = document.getElementById("passwordStrengthText");
    const passwordStrengthBar = document.getElementById("passwordStrengthBar");
    const pwLength = document.getElementById("pw-length");
    const pwUpper = document.getElementById("pw-upper");
    const pwLower = document.getElementById("pw-lower");
    const pwNumber = document.getElementById("pw-number");
    const pwSpecial = document.getElementById("pw-special");
    const confirmPasswordInput = document.getElementById("confirmPassword");
    const passwordMatchMsg = document.getElementById("passwordMatchMsg");
    const form = document.querySelector("form");
    const signUpBtn = document.getElementById("signUpBtn");
    // Required fields (excluding state)
    const requiredFields = [
        document.getElementById("firstName"),
        document.getElementById("lastName"),
        document.getElementById("email"),
        passwordInput,
        confirmPasswordInput,
        document.getElementById("countryInput"),
    ];
    const termsCheckbox = form.querySelector('input[type="checkbox"]');

    // Phone country code dropdown logic
    const phoneCountryDropdown = document.getElementById("phoneCountryDropdown");
    const phoneCountryFlag = document.getElementById("phoneCountryFlag");
    const phoneCountryCode = document.getElementById("phoneCountryCode");
    const phoneCountryArrow = document.getElementById("phoneCountryArrow");
    const phoneCountryOptions = document.getElementById("phoneCountryOptions");
    let phoneCountryList = [];
    let isPhoneCountryOpen = false;
    let selectedPhoneCountry = null;

    // Get the eye icons and input fields
    const togglePassword = document.getElementById('togglePassword');
    const toggleConfirmPassword = document.getElementById('toggleConfirmPassword');
    const passwordField = document.getElementById('password');
    const confirmPasswordField = document.getElementById('confirmPassword');





    async function fetchCountries() {
        try {
            const res = await fetch(
                "https://countriesnow.space/api/v0.1/countries/states"
            );
            const data = await res.json();
            countryList = data.data.sort((a, b) =>
                a.name.localeCompare(b.name)
            );
            renderCountryOptions();
        } catch {
            countryList = [
                { iso2: "US", name: "United States", states: [] },
                { iso2: "CA", name: "Canada", states: [] },
                { iso2: "GB", name: "United Kingdom", states: [] },
                { iso2: "IN", name: "India", states: [] },
                { iso2: "AU", name: "Australia", states: [] },
            ];
            renderCountryOptions();
        }
    }

    function renderCountryOptions() {
        countryOptions.innerHTML = "";
        countryList.forEach((c) => {
            const li = document.createElement("li");
            li.textContent = c.name;
            li.className =
                "px-4 py-2 hover:bg-teal-50 hover:text-[#1CB5BD] cursor-pointer text-gray-700 text-sm transition-all duration-200";
            li.tabIndex = 0;
            li.addEventListener("click", () =>
                selectCountry(c.iso2, c.name, li)
            );
            li.addEventListener("keydown", (e) => {
                if (e.key === "Enter" || e.key === " ") {
                    selectCountry(c.iso2, c.name, li);
                }
            });
            countryOptions.appendChild(li);
        });
    }

    function renderStateOptions(states) {
        stateOptions.innerHTML = "";
        if (!states || !states.length) {
            const li = document.createElement("li");
            li.textContent = "No states available";
            li.className = "px-4 py-2 text-gray-400 text-sm";
            stateOptions.appendChild(li);
            return;
        }
        states.forEach((s) => {
            const li = document.createElement("li");
            li.textContent = s.name;
            li.className =
                "px-4 py-2 hover:bg-teal-50 hover:text-[#1CB5BD] cursor-pointer text-gray-700 text-sm transition-all duration-200";
            li.tabIndex = 0;
            li.addEventListener("click", () => selectState(s.code, s.name, li));
            li.addEventListener("keydown", (e) => {
                if (e.key === "Enter" || e.key === " ") {
                    selectState(s.code, s.name, li);
                }
            });
            stateOptions.appendChild(li);
        });
    }

    function openCountryDropdown() {
        countryOptions.classList.remove("hidden");
        countryArrow.classList.add("rotate-180");
        isCountryOpen = true;
        updateCountryLabel();
    }
    function closeCountryDropdown() {
        countryOptions.classList.add("hidden");
        countryArrow.classList.remove("rotate-180");
        isCountryOpen = false;
        updateCountryLabel();
    }
    function selectCountry(code, name, li) {
        countrySelected.textContent = name;
        countryInput.value = code;
        Array.from(countryOptions.children).forEach((opt) =>
            opt.classList.remove(
                "bg-teal-100",
                "text-[#1CB5BD]",
                "font-semibold"
            )
        );
        if (li)
            li.classList.add("bg-teal-100", "text-[#1CB5BD]", "font-semibold");
        closeCountryDropdown();
        // Enable state field and populate
        stateField.classList.remove("opacity-50", "pointer-events-none");
        stateSelected.textContent = "Select State";
        stateInput.value = "";
        const countryObj = countryList.find((c) => c.iso2 === code);
        if (countryObj && countryObj.states && countryObj.states.length) {
            renderStateOptions(
                countryObj.states.map((s) => ({
                    code: s.state_code,
                    name: s.name,
                }))
            );
        } else {
            renderStateOptions([]);
        }

        // Sync with phone country code
        const phoneCountry = phoneCountryList.find((c) => c.cca2 === code);
        if (phoneCountry) {
            selectPhoneCountry(phoneCountry);
        }
    }
    function updateCountryLabel() {
        if (isCountryOpen) {
            countryLabel.classList.remove(
                "opacity-0",
                "text-gray-500",
                "top-2"
            );
            countryLabel.classList.add("opacity-100", "-top-2", "text-primary");
        } else {
            countryLabel.classList.add("opacity-0", "text-gray-500", "top-2");
            countryLabel.classList.remove(
                "opacity-100",
                "-top-2.5",
                "text-primary"
            );
        }
    }

    // State Dropdown Logic
    function openStateDropdown() {
        if (stateField.classList.contains("opacity-50")) return;
        stateOptions.classList.remove("hidden");
        stateArrow.classList.add("rotate-180");
        isStateOpen = true;
        updateStateLabel();
    }
    function closeStateDropdown() {
        stateOptions.classList.add("hidden");
        stateArrow.classList.remove("rotate-180");
        isStateOpen = false;
        updateStateLabel();
    }
    function selectState(code, name, li) {
        stateSelected.textContent = name;
        stateInput.value = code;
        Array.from(stateOptions.children).forEach((opt) =>
            opt.classList.remove(
                "bg-teal-100",
                "text-[#1CB5BD]",
                "font-semibold"
            )
        );
        if (li)
            li.classList.add("bg-teal-100", "text-[#1CB5BD]", "font-semibold");
        closeStateDropdown();
    }
    function updateStateLabel() {
        if (isStateOpen) {
            stateLabel.classList.remove("opacity-0", "text-gray-500", "top-2");
            stateLabel.classList.add("opacity-100", "-top-2", "text-primary");
        } else {
            stateLabel.classList.add("opacity-0", "text-gray-500", "top-2");
            stateLabel.classList.remove(
                "opacity-100",
                "-top-2.5",
                "text-primary"
            );
        }
    }

    // Country events
    countryField.addEventListener("click", () => {
        if (isCountryOpen) {
            closeCountryDropdown();
        } else {
            openCountryDropdown();
        }
    });
    countryField.addEventListener("focus", () => {
        openCountryDropdown();
    });
    countryField.addEventListener("blur", () => {
        setTimeout(() => {
            closeCountryDropdown();
        }, 100);
    });
    document.addEventListener("click", (e) => {
        if (!countryDropdown.contains(e.target)) {
            closeCountryDropdown();
        }
        if (!stateDropdown.contains(e.target)) {
            closeStateDropdown();
        }
    });
    
    // State events
    stateField.addEventListener("click", () => {
        if (stateField.classList.contains("opacity-50")) return;
        if (isStateOpen) {
            closeStateDropdown();
        } else {
            openStateDropdown();
        }
    });
    stateField.addEventListener("focus", () => {
        if (!stateField.classList.contains("opacity-50")) openStateDropdown();
    });
    stateField.addEventListener("blur", () => {
        setTimeout(() => {
            closeStateDropdown();
        }, 100);
    });
    


    // If the user clicks outside of the input, hide the strength box
    document.addEventListener('click', function (event) {
        if (!passwordInput.contains(event.target) && !passwordStrengthBox.contains(event.target)) {
            passwordStrengthBox.classList.add("hidden");
        }
    });

    

    async function fetchPhoneCountries() {
        try {
            const res = await fetch("https://restcountries.com/v3.1/all");
            const data = await res.json();
            phoneCountryList = data
                .map((c) => ({
                    name: c.name.common,
                    code: c.idd?.root
                        ? c.idd.root + (c.idd.suffixes ? c.idd.suffixes[0] : "")
                        : "",
                    flag: c.flags?.svg || c.flags?.png || "",
                    cca2: c.cca2,
                }))
                .filter((c) => c.code && c.flag)
                .sort((a, b) => a.name.localeCompare(b.name));
            renderPhoneCountryOptions();
            // Default to US if available
            const us = phoneCountryList.find((c) => c.cca2 === "US");
            if (us) selectPhoneCountry(us);
        } catch {
            // fallback
            phoneCountryList = [
                {
                    name: "United States",
                    code: "+1",
                    flag: "https://flagcdn.com/us.svg",
                    cca2: "US",
                },
                {
                    name: "United Kingdom",
                    code: "+44",
                    flag: "https://flagcdn.com/gb.svg",
                    cca2: "GB",
                },
                {
                    name: "Canada",
                    code: "+1",
                    flag: "https://flagcdn.com/ca.svg",
                    cca2: "CA",
                },
                {
                    name: "Australia",
                    code: "+61",
                    flag: "https://flagcdn.com/au.svg",
                    cca2: "AU",
                },
                {
                    name: "India",
                    code: "+91",
                    flag: "https://flagcdn.com/in.svg",
                    cca2: "IN",
                },
            ];
            renderPhoneCountryOptions();
            selectPhoneCountry(phoneCountryList[0]);
        }
    }

    function renderPhoneCountryOptions(filter = "") {
        phoneCountryOptions.innerHTML = "";
        let filtered = phoneCountryList;
        if (filter) {
            const f = filter.trim().toLowerCase();
            filtered = phoneCountryList.filter(
                (c) =>
                    c.name.toLowerCase().includes(f) ||
                    c.code.replace("+", "").startsWith(f.replace("+", "")) ||
                    c.cca2.toLowerCase().startsWith(f)
            );
        }
        filtered.forEach((c) => {
            const li = document.createElement("li");
            li.className =
                "flex items-center px-3 py-2 hover:bg-teal-50 hover:text-[#1CB5BD] cursor-pointer";
            li.innerHTML = `<img src="${c.flag}" alt="${c.cca2}" class="w-5 h-5 mr-2 rounded-sm"/> <span class="font-medium">${c.name}</span> <span class="ml-auto text-gray-500">${c.code}</span>`;
            li.addEventListener("click", () => selectPhoneCountry(c));
            phoneCountryOptions.appendChild(li);
        });
    }

    function selectPhoneCountry(c) {
        selectedPhoneCountry = c;
        phoneCountryFlag.innerHTML = `<img src="${c.flag}" alt="${c.cca2}" class="w-5 h-5 rounded-sm"/>`;
        phoneCountryCode.value = c.code;
        closePhoneCountryDropdown();

        // Find and select the corresponding country in the main country dropdown
        const countryObj = countryList.find(
            (country) => country.iso2 === c.cca2
        );
        if (countryObj) {
            const countryOption = Array.from(countryOptions.children).find(
                (li) => li.textContent === countryObj.name
            );
            if (countryOption) {
                selectCountry(countryObj.iso2, countryObj.name, countryOption);
            }
        }
    }

    function openPhoneCountryDropdown() {
        phoneCountryOptions.classList.remove("hidden");
        phoneCountryArrow.classList.add("rotate-180");
        isPhoneCountryOpen = true;
    }

    function closePhoneCountryDropdown() {
        phoneCountryOptions.classList.add("hidden");
        phoneCountryArrow.classList.remove("rotate-180");
        isPhoneCountryOpen = false;
    }

    phoneCountryDropdown.addEventListener("click", (e) => {
        e.stopPropagation();
        if (isPhoneCountryOpen) {
            closePhoneCountryDropdown();
        } else {
            openPhoneCountryDropdown();
            renderPhoneCountryOptions();
        }
    });

    phoneCountryCode.addEventListener("input", (e) => {
        renderPhoneCountryOptions(e.target.value);
        openPhoneCountryDropdown();
    });

    phoneCountryOptions.addEventListener("click", (e) => {
        e.stopPropagation();
    });

    document.addEventListener("click", (e) => {
        if (!phoneCountryDropdown.contains(e.target)) {
            closePhoneCountryDropdown();
        }
    });

    // Add keyboard navigation for phone country dropdown
    phoneCountryDropdown.addEventListener("keydown", function (e) {
        if (e.key === "ArrowDown") {
            e.preventDefault();
            openPhoneCountryDropdown();
            const first = phoneCountryOptions.querySelector("li");
            if (first) first.focus();
        }
    });

    phoneCountryOptions.addEventListener("keydown", function (e) {
        const active = document.activeElement;
        if (e.key === "ArrowDown") {
            e.preventDefault();
            if (active.nextElementSibling) active.nextElementSibling.focus();
        } else if (e.key === "ArrowUp") {
            e.preventDefault();
            if (active.previousElementSibling)
                active.previousElementSibling.focus();
        } else if (e.key === "Escape") {
            closePhoneCountryDropdown();
            phoneCountryDropdown.focus();
        }
    });





    function checkPasswordMatch() {
        if (confirmPasswordInput.value === "") {
            passwordMatchMsg.classList.add("hidden");
            confirmPasswordInput.classList.remove("border-red-500");
            checkFormValidity();
            return true;
        }
        if (passwordInput.value !== confirmPasswordInput.value) {
            passwordMatchMsg.classList.remove("hidden");
            confirmPasswordInput.classList.add("border-red-500");
            checkFormValidity();
            return false;
        } else {
            passwordMatchMsg.classList.add("hidden");
            confirmPasswordInput.classList.remove("border-red-500");
            checkFormValidity();
            return true;
        }
    }
    function checkFormValidity() {
        const allFilled = requiredFields.every(
            (f) => f && f.value && f.value.trim() !== ""
        );
        const passwordsMatch =
            passwordInput.value === confirmPasswordInput.value &&
            passwordInput.value !== "";
        const termsChecked = termsCheckbox && termsCheckbox.checked;
        if (allFilled && passwordsMatch && termsChecked) {
            signUpBtn.disabled = false;
        } else {
            signUpBtn.disabled = true;
        }
    }
    requiredFields.forEach(
        (f) => f && f.addEventListener("input", checkFormValidity)
    );
    if (termsCheckbox)
        termsCheckbox.addEventListener("change", checkFormValidity);
    // Also check on country selection
    document
        .getElementById("countryField")
        .addEventListener("click", () => setTimeout(checkFormValidity, 100));
    // Initial check
    checkFormValidity();
    passwordInput.addEventListener("input", checkPasswordMatch);
    confirmPasswordInput.addEventListener("input", checkPasswordMatch);
    form.addEventListener("submit", function (e) {
        if (!checkPasswordMatch()) {
            e.preventDefault();
            confirmPasswordInput.focus();
        }
    });

    function getPasswordStrengthDetails(pw) {
        const checks = {
            length: pw.length >= 8,
            upper: /[A-Z]/.test(pw),
            lower: /[a-z]/.test(pw),
            number: /[0-9]/.test(pw),
            special: /[^A-Za-z0-9]/.test(pw),
        };
        const passed = Object.values(checks).filter(Boolean).length;
        let strength = {
            text: "Weak",
            color: "text-red-500",
            bar: "bg-red-400",
            width: "20%",
        };
        if (passed === 5 && pw.length >= 12) {
            strength = {
                text: "Very Strong",
                color: "text-green-600",
                bar: "bg-green-500",
                width: "100%",
            };
        } else if (passed >= 4) {
            strength = {
                text: "Strong",
                color: "text-green-600",
                bar: "bg-green-400",
                width: "80%",
            };
        } else if (passed >= 3) {
            strength = {
                text: "Medium",
                color: "text-yellow-500",
                bar: "bg-yellow-400",
                width: "60%",
            };
        } else if (passed >= 2) {
            strength = {
                text: "Weak",
                color: "text-orange-500",
                bar: "bg-orange-400",
                width: "40%",
            };
        }
        return { ...strength, checks };
    }
    passwordInput.addEventListener("input", function () {
        const val = passwordInput.value;
        if (!val) {
            passwordStrengthBox.classList.add("hidden");
            passwordStrengthText.textContent = "";
            passwordStrengthBar.style.width = "0%";
            return;
        }
        // show password strength box
        const strength = getPasswordStrengthDetails(val);
        passwordStrengthBox.classList.remove("hidden");
        passwordStrengthText.textContent = strength.text;
        passwordStrengthText.className = strength.color + " font-semibold";
        passwordStrengthBar.className =
            "h-2 rounded-full transition-all duration-300 " + strength.bar;
        passwordStrengthBar.style.width = strength.width;
        passwordStrengthBox.style.width = strength.width;
        passwordStrengthBox.style.minWidth = "180px"; // Prevent it from being too tiny
        passwordStrengthBox.style.maxWidth = "300px"; // Prevent it from stretching too far

        // Checklist
        pwLength.className =
            "flex items-center " +
            (strength.checks.length ? "text-green-600" : "text-red-500");
        pwLength.children[0].textContent = strength.checks.length ? "✓" : "✗";
        pwUpper.className =
            "flex items-center " +
            (strength.checks.upper ? "text-green-600" : "text-red-500");
        pwUpper.children[0].textContent = strength.checks.upper ? "✓" : "✗";
        pwLower.className =
            "flex items-center " +
            (strength.checks.lower ? "text-green-600" : "text-red-500");
        pwLower.children[0].textContent = strength.checks.lower ? "✓" : "✗";
        pwNumber.className =
            "flex items-center " +
            (strength.checks.number ? "text-green-600" : "text-red-500");
        pwNumber.children[0].textContent = strength.checks.number ? "✓" : "✗";
        pwSpecial.className =
            "flex items-center " +
            (strength.checks.special ? "text-green-600" : "text-red-500");
        pwSpecial.children[0].textContent = strength.checks.special
            ? "✓"
            : "✗";
    });

    // Function to toggle password visibility
    function togglePasswordVisibility(inputField, icon) {
        const type = inputField.type === 'password' ? 'text' : 'password';
        inputField.type = type;

        // Toggle the eye icon between "eye" and "eye-slash"
        icon.classList.toggle('fa-eye');
        icon.classList.toggle('fa-eye-slash');
    }

    // Add event listeners to the eye icons
    togglePassword.addEventListener('click', () => togglePasswordVisibility(passwordField, togglePassword));
    toggleConfirmPassword.addEventListener('click', () => togglePasswordVisibility(confirmPasswordField, toggleConfirmPassword));


    fetchPhoneCountries();
    fetchCountries();
    updateCountryLabel();
    updateStateLabel();
});