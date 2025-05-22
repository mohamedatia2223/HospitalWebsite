import { GetAllReviewsForDoctorById, getDoctorById, getDoctorPatients } from "../assets/js/api/doctors.js";
import { getToken } from "../assets/js/utils/jwt.js";
import { setupUser } from "../assets/js/authSetup.js";

// authenticating
const token = getToken();
setupUser();

// Initialize Feather Icons
// feather.replace();

// Tab Switching Logic with localStorage persistence
function setupProfileTabs() {
	// Get all tab elements
	const tabs = document.querySelectorAll(".profile-tab");
	const tabContents = document.querySelectorAll(".tab-content");

	// Function to activate a tab
	function activateTab(tabId) {
		// Remove active class from all tabs and contents
		tabs.forEach((t) => t.classList.remove("active"));
		tabContents.forEach((c) => c.classList.remove("active"));

		// Add active class to clicked tab and corresponding content
		const tab = document.querySelector(`.profile-tab[data-tab="${tabId}"]`);
		const content = document.getElementById(tabId);

		if (tab && content) {
			tab.classList.add("active");
			content.classList.add("active");

			// Store the active tab in localStorage
			localStorage.setItem("activeTab", tabId);
		}
	}

	// Check for saved tab on page load
	const savedTab = localStorage.getItem("activeTab");
	if (savedTab) {
		activateTab(savedTab);
	} else {
		// Default to first tab if none saved
		activateTab(tabs[0].dataset.tab);
	}

	// Add click event listeners
	tabs.forEach((tab) => {
		tab.addEventListener("click", () => {
			activateTab(tab.dataset.tab);
		});
	});
}

// Mobile Menu Functionality
function setupMobileMenu() {
	const mobileMenuButton = document.getElementById("mobile-menu-button");
	const mobileMenu = document.getElementById("mobile-menu");
	const mobileServicesMenu = document.getElementById("mobile-services-menu");
	const mobileDropdownContent = document.querySelector(".dropdown-content2");
	const menuIcon = document.getElementById("menu-icon");
	const servicesMobileArrow = document.getElementById(
		"services-mobile-arrow"
	);

	// Mobile profile and notification elements
	const mobileProfileBtn = document.getElementById("mobile-profile-btn");
	const mobileProfileDropdown = document.getElementById(
		"mobile-profile-dropdown"
	);
	const mobileNotificationBtn = document.getElementById(
		"mobile-notification-btn"
	);
	const mobileNotificationDropdown = document.getElementById(
		"mobile-notification-dropdown"
	);

	function handleScreenSize() {
		if (window.innerWidth >= 767) {
			// md breakpoint
			mobileMenu.classList.remove("active");
			mobileDropdownContent.classList.remove("active");
			menuIcon.classList.remove("ri-close-line");
			menuIcon.classList.add("ri-menu-line");
		}
	}

	// Initial check
	handleScreenSize();

	// Add resize event listener
	window.addEventListener("resize", handleScreenSize);

	// Toggle mobile menu
	mobileMenuButton.addEventListener("click", (e) => {
		e.stopPropagation();
		mobileMenu.classList.toggle("active");
		menuIcon.classList.toggle("ri-menu-line");
		menuIcon.classList.toggle("ri-close-line");
		// Close other dropdowns when menu opens
		if (mobileMenu.classList.contains("active")) {
			mobileProfileDropdown.classList.add("hidden");
			mobileNotificationDropdown.classList.add("hidden");
		}
	});

	// Services dropdown toggle
	mobileServicesMenu.addEventListener("click", function (e) {
		e.preventDefault();
		e.stopPropagation();
		mobileDropdownContent.classList.toggle("active");
		servicesMobileArrow.classList.toggle("rotate-180");
	});

	// Mobile profile dropdown toggle
	mobileProfileBtn.addEventListener("click", (e) => {
		e.stopPropagation();
		mobileProfileDropdown.classList.toggle("hidden");
		mobileNotificationDropdown.classList.add("hidden");
		mobileMenu.classList.remove("active");
		menuIcon.classList.remove("ri-close-line");
		menuIcon.classList.add("ri-menu-line");
	});

	// Mobile notification dropdown toggle
	mobileNotificationBtn.addEventListener("click", (e) => {
		e.stopPropagation();
		mobileNotificationDropdown.classList.toggle("hidden");
		mobileProfileDropdown.classList.add("hidden");
		mobileMenu.classList.remove("active");
		menuIcon.classList.remove("ri-close-line");
		menuIcon.classList.add("ri-menu-line");
	});

	// Close dropdowns when clicking outside
	document.addEventListener("click", (e) => {
		if (
			!mobileProfileBtn.contains(e.target) &&
			!mobileProfileDropdown.contains(e.target)
		) {
			mobileProfileDropdown.classList.add("hidden");
		}
		if (
			!mobileNotificationBtn.contains(e.target) &&
			!mobileNotificationDropdown.contains(e.target)
		) {
			mobileNotificationDropdown.classList.add("hidden");
		}
		if (
			!mobileMenuButton.contains(e.target) &&
			!mobileMenu.contains(e.target)
		) {
			mobileMenu.classList.remove("active");
			menuIcon.classList.remove("ri-close-line");
			menuIcon.classList.add("ri-menu-line");
			mobileDropdownContent.classList.remove("active");
			servicesMobileArrow.classList.remove("rotate-180");
		}
	});

	// Prevent dropdowns from closing when clicking inside
	mobileProfileDropdown.addEventListener("click", (e) => {
		e.stopPropagation();
	});

	mobileNotificationDropdown.addEventListener("click", (e) => {
		e.stopPropagation();
	});
}

// Appointment Management
function setupAppointmentManagement() {
	let appointmentToRemove = null;

	function showModal(row) {
		appointmentToRemove = row;
		document.getElementById("confirmationModal").classList.remove("hidden");
	}

	function hideModal() {
		document.getElementById("confirmationModal").classList.add("hidden");
	}

	function showAlert(message, type = "success", duration = 4000) {
		const container = document.getElementById("alert-container");

		// Icon SVGs
		const icons = {
			success: `<i class="fas fa-check-circle text-green-700"></i>`,
			error: `<i class="fas fa-times-circle text-red-700"></i>`,
			warning: `<i class="fas fa-exclamation-triangle text-yellow-700"></i>`,
			info: `<i class="fas fa-info-circle text-blue-700"></i>`,
		};

		// Color mapping
		const colors = {
			success: {
				bg: "bg-green-100",
				text: "text-green-700",
				strip: "bg-green-600",
				progress: "bg-green-600",
				hover: "hover:bg-green-200",
			},
			error: {
				bg: "bg-red-100",
				text: "text-red-700",
				strip: "bg-red-600",
				progress: "bg-red-600",
				hover: "hover:bg-red-200",
			},
			warning: {
				bg: "bg-yellow-100",
				text: "text-yellow-800",
				strip: "bg-yellow-600",
				progress: "bg-yellow-600",
				hover: "hover:bg-yellow-200",
			},
			info: {
				bg: "bg-blue-100",
				text: "text-blue-700",
				strip: "bg-blue-600",
				progress: "bg-blue-600",
				hover: "hover:bg-blue-200",
			},
		};

		const { bg, text, strip, progress, hover } =
			colors[type] || colors.success;
		const icon = icons[type] || "";

		// Create alert element
		const alert = document.createElement("div");
		alert.className = `relative flex items-stretch shadow-md rounded-md overflow-hidden opacity-0 transition-opacity duration-300 ${bg} ${text}`;
		alert.innerHTML = `
            <!-- Left Strip -->
            <div class="w-2 ${strip}"></div>

            <!-- Icon & Message -->
            <div class="flex-1 flex items-center gap-2 px-4 py-3">
                ${icon}
                <span class="text-sm">${message}</span>
            </div>

            <!-- Close button -->
            <button class="absolute top-2.5 right-2.5 w-6 h-6 rounded-full flex items-center justify-center ${hover} transition duration-200">
                <i class="fas fa-times text-sm text-gray-500 hover:text-gray-600 transition-colors font-bold "></i>
            </button>

            <!-- Progress bar -->
            <div class="absolute bottom-0 left-0 h-1 ${progress}" style="width: 100%; transition: width ${duration}ms linear;"></div>
            `;

		container.appendChild(alert);

		// Trigger fade-in
		requestAnimationFrame(() => {
			alert.classList.remove("opacity-0");
			alert.querySelector("div.absolute.bottom-0").style.width = "0%";
		});

		// Auto-remove after time
		const timeout = setTimeout(() => removeAlert(), duration);

		// Manual close
		alert.querySelector("button").addEventListener("click", () => {
			clearTimeout(timeout);
			removeAlert();
		});

		function removeAlert() {
			alert.classList.add("opacity-0");
			setTimeout(() => {
				alert.remove();
			}, 300);
		}
	}

	// Add click handlers to all decline buttons
	document.querySelectorAll(".decline-btn").forEach((button) => {
		button.addEventListener("click", function (e) {
			e.preventDefault();
			const row = this.closest("tr");
			showModal(row);
		});
	});

	// Modal button handlers
	document.getElementById("closeModal").addEventListener("click", hideModal);
	document
		.getElementById("cancelAction")
		.addEventListener("click", hideModal);

	document
		.getElementById("confirmAction")
		.addEventListener("click", function () {
			if (appointmentToRemove) {
				appointmentToRemove.remove();
				showAlert(
					"Appointment cancelled successfully",
					"success",
					4000
				);
				hideModal();
			}
		});
}

// Desktop Dropdowns
function setupDesktopDropdowns() {
	// Services dropdown
	const servicesMenu = document.getElementById("services-menu");
	const servicesDropdown = document.getElementById("services-dropdown");
	const servicesArrow = document.getElementById("services-arrow");

	if (servicesMenu) {
		servicesMenu.addEventListener("click", function (e) {
			e.preventDefault();
			servicesDropdown.classList.toggle("hidden");
			servicesArrow.classList.toggle("rotate-180");
		});

		document.addEventListener("click", function (e) {
			if (
				!servicesMenu.contains(e.target) &&
				!servicesDropdown.contains(e.target)
			) {
				servicesDropdown.classList.add("hidden");
				servicesArrow.classList.remove("rotate-180");
			}
		});
	}

	// Profile dropdown
	const profileBtn = document.getElementById("profile-btn");
	const profileDropdown = document.getElementById("profile-dropdown");
	const notificationBtn = document.getElementById("notification-btn");
	const notificationDropdown = document.getElementById(
		"notification-dropdown"
	);

	if (profileBtn) {
		profileBtn.addEventListener("click", function (e) {
			e.stopPropagation();
			profileDropdown.classList.toggle("hidden");
			notificationDropdown.classList.add("hidden");
		});
	}

	if (notificationBtn) {
		notificationBtn.addEventListener("click", function (e) {
			e.stopPropagation();
			notificationDropdown.classList.toggle("hidden");
			profileDropdown.classList.add("hidden");
		});
	}

	document.addEventListener("click", function () {
		if (profileDropdown) profileDropdown.classList.add("hidden");
		if (notificationDropdown) notificationDropdown.classList.add("hidden");
	});

	if (profileDropdown) {
		profileDropdown.addEventListener("click", function (e) {
			e.stopPropagation();
		});
	}

	if (notificationDropdown) {
		notificationDropdown.addEventListener("click", function (e) {
			e.stopPropagation();
		});
	}
}

// Detect overflowing containers and add 'overflowing' class
function setupScrollbars() {
	const containers = document.querySelectorAll(".dynamic-scrollbar");

	containers.forEach((container) => {
		function checkOverflow() {
			if (container.scrollHeight > container.clientHeight) {
				container.classList.add("overflowing");
			} else {
				container.classList.remove("overflowing");
			}
		}

		// Check initially
		checkOverflow();

		// Check when window resizes
		window.addEventListener("resize", checkOverflow);

		// Check when content changes (for dynamic content like notes)
		const observer = new MutationObserver(checkOverflow);
		observer.observe(container, { childList: true, subtree: true });
	});
}

// Calendar functionality
function setupCalendar() {
	const calendarDays = document.getElementById("calendar-days");
	const currentMonthElement = document.getElementById("current-month");
	const prevMonthButton = document.getElementById("prev-month");
	const nextMonthButton = document.getElementById("next-month");

	let currentDate = new Date();
	let currentMonth = currentDate.getMonth();
	let currentYear = currentDate.getFullYear();

	// For demo purposes, set to a future date where we have data
	currentDate = new Date(2025, 3, 15); // April 15, 2025
	currentMonth = currentDate.getMonth();
	currentYear = currentDate.getFullYear();

	// Doctor availability data
	const doctorAvailability = {
		// Format: 'YYYY-MM-DD': { morning: true/false, afternoon: true/false, evening: true/false }
		"2025-04-15": { morning: true, afternoon: true, evening: false },
		"2025-04-16": { morning: true, afternoon: true, evening: false },
		"2025-04-17": { morning: false, afternoon: true, evening: true },
		"2025-04-18": { morning: true, afternoon: false, evening: false },
		"2025-04-19": { morning: false, afternoon: false, evening: false }, // Weekend
		"2025-04-20": { morning: false, afternoon: false, evening: false }, // Weekend
		"2025-04-21": { morning: true, afternoon: true, evening: false },
		"2025-04-22": { morning: true, afternoon: true, evening: false },
		"2025-04-23": { morning: true, afternoon: false, evening: true },
		"2025-04-24": { morning: false, afternoon: true, evening: false },
		"2025-04-25": { morning: true, afternoon: true, evening: false },
	};

	// Sample appointments
	const appointments = [
		{
			id: "APT-001",
			date: "2025-04-15",
			time: "10:00 AM",
			patientName: "Milla Willow",
			purpose: "Hypertension Follow-up",
			status: "confirmed",
		},
		{
			id: "APT-002",
			date: "2025-04-16",
			time: "2:30 PM",
			patientName: "John Smith",
			purpose: "Diabetes Checkup",
			status: "confirmed",
		},
		{
			id: "APT-003",
			date: "2025-04-17",
			time: "11:15 AM",
			patientName: "Emma Johnson",
			purpose: "Asthma Consultation",
			status: "pending",
		},
	];

	function updateCalendar() {
		// Update month display
		currentMonthElement.textContent = new Date(
			currentYear,
			currentMonth,
			1
		).toLocaleDateString("en-US", {
			month: "long",
			year: "numeric",
		});

		// Clear calendar
		calendarDays.innerHTML = "";

		// Get first day of month
		const firstDay = new Date(currentYear, currentMonth, 1).getDay();

		// Get last day of month
		const lastDay = new Date(currentYear, currentMonth + 1, 0).getDate();

		// Get current day (for highlighting today)
		const today = new Date();
		const isCurrentMonth =
			today.getMonth() === currentMonth &&
			today.getFullYear() === currentYear;
		const todayDate = today.getDate();

		// Add empty cells for days before first day of month
		for (let i = 0; i < firstDay; i++) {
			const emptyCell = document.createElement("div");
			emptyCell.className = "bg-white p-2 min-h-[80px]";
			calendarDays.appendChild(emptyCell);
		}

		// Add cells for each day of the month
		for (let day = 1; day <= lastDay; day++) {
			const cell = document.createElement("div");
			cell.className = "bg-white p-2 min-h-[80px] relative";

			// Highlight current day
			if (isCurrentMonth && day === todayDate) {
				cell.classList.add("bg-blue-50");
			}

			// Date number
			const dateNumber = document.createElement("div");
			dateNumber.className = "text-right mb-1";
			dateNumber.textContent = day;
			cell.appendChild(dateNumber);

			// Format date for checking availability
			const dateString = `${currentYear}-${String(
				currentMonth + 1
			).padStart(2, "0")}-${String(day).padStart(2, "0")}`;

			// Check for availability and appointments
			const availability = doctorAvailability[dateString];
			const dayAppointments = appointments.filter(
				(apt) => apt.date === dateString
			);

			// Availability indicators
			if (availability) {
				const availabilityContainer = document.createElement("div");
				availabilityContainer.className =
					"flex flex-col space-y-1 mt-1";

				if (availability.morning) {
					const morningIndicator = document.createElement("div");
					morningIndicator.className =
						"text-xs px-1 py-0.5 rounded bg-blue-100 text-blue-800 text-center";
					morningIndicator.textContent = "AM";
					availabilityContainer.appendChild(morningIndicator);
				}

				if (availability.afternoon) {
					const afternoonIndicator = document.createElement("div");
					afternoonIndicator.className =
						"text-xs px-1 py-0.5 rounded bg-blue-100 text-blue-800 text-center";
					afternoonIndicator.textContent = "PM";
					availabilityContainer.appendChild(afternoonIndicator);
				}

				if (availability.evening) {
					const eveningIndicator = document.createElement("div");
					eveningIndicator.className =
						"text-xs px-1 py-0.5 rounded bg-blue-100 text-blue-800 text-center";
					eveningIndicator.textContent = "EVE";
					availabilityContainer.appendChild(eveningIndicator);
				}

				cell.appendChild(availabilityContainer);
			}

			// Add appointment indicators if there are any
			if (dayAppointments.length > 0) {
				const appointmentCount = dayAppointments.length;
				const maxToShow = 2;

				for (
					let i = 0;
					i < Math.min(appointmentCount, maxToShow);
					i++
				) {
					const apt = dayAppointments[i];
					const apptIndicator = document.createElement("div");

					// Style based on status
					if (apt.status === "confirmed") {
						apptIndicator.className =
							"mt-1 text-xs px-1 py-0.5 rounded bg-green-100 text-green-800 truncate";
					} else if (apt.status === "pending") {
						apptIndicator.className =
							"mt-1 text-xs px-1 py-0.5 rounded bg-yellow-100 text-yellow-800 truncate";
					} else {
						apptIndicator.className =
							"mt-1 text-xs px-1 py-0.5 rounded bg-gray-100 text-gray-800 truncate";
					}

					apptIndicator.textContent = `${apt.time}: ${
						apt.patientName.split(" ")[0]
					}`;
					cell.appendChild(apptIndicator);
				}

				// Show more indicator if there are more appointments
				if (appointmentCount > maxToShow) {
					const moreIndicator = document.createElement("div");
					moreIndicator.className =
						"mt-1 text-xs text-center text-gray-500";
					moreIndicator.textContent = `+${
						appointmentCount - maxToShow
					} more`;
					cell.appendChild(moreIndicator);
				}
			}

			// Add click handler to show day detail
			cell.addEventListener("click", function () {
				showDayDetail(dateString);
			});

			calendarDays.appendChild(cell);
		}

		// Update today's appointments section
		updateTodayAppointments();
	}

	// Show appointment details for a specific day
	function showDayDetail(dateString) {
		// Format date for display
		const dateObj = new Date(dateString);
		const formattedDate = dateObj.toLocaleDateString("en-US", {
			weekday: "long",
			month: "long",
			day: "numeric",
			year: "numeric",
		});

		// Get appointments for the selected day
		const dayAppointments = appointments.filter(
			(apt) => apt.date === dateString
		);

		// Get availability for the selected day
		const availability = doctorAvailability[dateString] || {
			morning: false,
			afternoon: false,
			evening: false,
		};

		// Create modal
		const modal = document.createElement("div");
		modal.className =
			"fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50";

		// Format availability text
		let availabilityText = "";
		if (availability.morning) availabilityText += "Morning, ";
		if (availability.afternoon) availabilityText += "Afternoon, ";
		if (availability.evening) availabilityText += "Evening, ";
		availabilityText = availabilityText.replace(/, $/, "");

		if (!availabilityText) {
			availabilityText = "Not available";
		}

		modal.innerHTML = `
                <div class="bg-white rounded-lg p-6 max-w-lg w-full">
                    <div class="flex justify-between items-center mb-4">
                        <h3 class="text-lg font-semibold text-gray-800">${formattedDate}</h3>
                        <button class="close-modal text-gray-500 hover:text-gray-700">
                            <i class="ri-close-line"></i>
                        </button>
                    </div>
                    <div class="mb-4">
                        <p class="text-sm text-gray-700"><strong>Availability:</strong> ${availabilityText}</p>
                    </div>
                    <div class="mb-6">
                        <h4 class="text-md font-medium text-gray-800 mb-3">Appointments (${
							dayAppointments.length
						})</h4>
                        ${
							dayAppointments.length === 0
								? '<p class="text-sm text-gray-500">No appointments scheduled for this day.</p>'
								: `<div class="space-y-3">${dayAppointments
										.map(
											(apt) => `
                                <div class="flex items-center p-3 border border-gray-200 rounded-md">
                                    <div class="flex-shrink-0 h-10 w-10">
                                        <img class="h-10 w-10 rounded-full object-cover" src="../assets/images/patients/${apt.patientName
											.toLowerCase()
											.replace(" ", "-")}.jpg" alt="${
												apt.patientName
											}">
                                    </div>
                                    <div class="ml-4 flex-grow">
                                        <div class="flex justify-between">
                                            <div>
                                                <p class="text-sm font-medium text-gray-900">${
													apt.patientName
												}</p>
                                                <p class="text-xs text-gray-500">${
													apt.time
												} - ${apt.purpose}</p>
                                            </div>
                                            <span class="px-2 inline-flex text-xs leading-5 font-semibold rounded-full ${
												apt.status === "confirmed"
													? "bg-green-100 text-green-800"
													: apt.status === "pending"
													? "bg-yellow-100 text-yellow-800"
													: "bg-gray-100 text-gray-800"
											}">${apt.status}</span>
                                        </div>
                                    </div>
                                </div>
                            `
										)
										.join("")}</div>`
						}
                    </div>
                    <div class="flex justify-between items-center">
                        <button id="update-availability-btn" class="px-4 py-2 bg-primary text-white rounded-button hover:bg-primary-dark">
                            Update Availability
                        </button>
                        <button id="new-appointment-calendar-btn" class="px-4 py-2 border border-primary text-primary rounded-button hover:bg-primary-50" ${
							!availability.morning &&
							!availability.afternoon &&
							!availability.evening
								? "disabled"
								: ""
						}>
                            New Appointment
                        </button>
                    </div>
                </div>
            `;

		document.body.appendChild(modal);

		// Close modal event
		modal
			.querySelector(".close-modal")
			.addEventListener("click", function () {
				document.body.removeChild(modal);
			});

		// Update availability button
		modal
			.querySelector("#update-availability-btn")
			.addEventListener("click", function () {
				showUpdateAvailabilityModal(dateString, availability);
				document.body.removeChild(modal);
			});

		// New appointment button
		const newAppointmentBtn = modal.querySelector(
			"#new-appointment-calendar-btn"
		);
		if (!newAppointmentBtn.hasAttribute("disabled")) {
			newAppointmentBtn.addEventListener("click", function () {
				showNewAppointmentModal(dateString);
				document.body.removeChild(modal);
			});
		}
	}

	// Show modal to update availability
	function showUpdateAvailabilityModal(dateString, currentAvailability) {
		const dateObj = new Date(dateString);
		const formattedDate = dateObj.toLocaleDateString("en-US", {
			weekday: "long",
			month: "long",
			day: "numeric",
			year: "numeric",
		});

		const modal = document.createElement("div");
		modal.className =
			"fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50";
		modal.innerHTML = `
                <div class="bg-white rounded-lg p-6 max-w-md w-full">
                    <div class="flex justify-between items-center mb-4">
                        <h3 class="text-lg font-semibold text-gray-800">Update Availability: ${formattedDate}</h3>
                        <button class="close-modal text-gray-500 hover:text-gray-700">
                            <i class="ri-close-line"></i>
                        </button>
                    </div>
                    <div class="mb-6 space-y-4">
                        <div class="flex items-center">
                            <input type="checkbox" id="morning-availability" class="h-4 w-4 text-primary focus:ring-primary border-gray-300 rounded" ${
								currentAvailability.morning ? "checked" : ""
							}>
                            <label for="morning-availability" class="ml-2 block text-sm text-gray-700">Morning (8:00 AM - 12:00 PM)</label>
                        </div>
                        <div class="flex items-center">
                            <input type="checkbox" id="afternoon-availability" class="h-4 w-4 text-primary focus:ring-primary border-gray-300 rounded" ${
								currentAvailability.afternoon ? "checked" : ""
							}>
                            <label for="afternoon-availability" class="ml-2 block text-sm text-gray-700">Afternoon (1:00 PM - 5:00 PM)</label>
                        </div>
                        <div class="flex items-center">
                            <input type="checkbox" id="evening-availability" class="h-4 w-4 text-primary focus:ring-primary border-gray-300 rounded" ${
								currentAvailability.evening ? "checked" : ""
							}>
                            <label for="evening-availability" class="ml-2 block text-sm text-gray-700">Evening (6:00 PM - 9:00 PM)</label>
                        </div>
                        
                        <div class="flex items-center mt-4">
                            <input type="checkbox" id="apply-to-weekday" class="h-4 w-4 text-primary focus:ring-primary border-gray-300 rounded">
                            <label for="apply-to-weekday" class="ml-2 block text-sm text-gray-700">Apply to all ${dateObj.toLocaleDateString(
								"en-US",
								{ weekday: "long" }
							)}s this month</label>
                        </div>
                    </div>
                    <div class="flex justify-end space-x-4">
                        <button class="close-modal px-4 py-2 border border-gray-300 rounded-button text-gray-700 hover:bg-gray-50">
                            Cancel
                        </button>
                        <button id="save-availability" data-date="${dateString}" class="px-4 py-2 bg-primary text-white rounded-button hover:bg-primary-dark">
                            Save Changes
                        </button>
                    </div>
                </div>
            `;

		document.body.appendChild(modal);

		// Close modal event
		modal.querySelectorAll(".close-modal").forEach((button) => {
			button.addEventListener("click", function () {
				document.body.removeChild(modal);
			});
		});

		// Save availability event
		document
			.getElementById("save-availability")
			.addEventListener("click", function () {
				const dateStr = this.dataset.date;
				const applyToWeekday =
					document.getElementById("apply-to-weekday").checked;

				const newAvailability = {
					morning: document.getElementById("morning-availability")
						.checked,
					afternoon: document.getElementById("afternoon-availability")
						.checked,
					evening: document.getElementById("evening-availability")
						.checked,
				};

				// Update availability for this date
				doctorAvailability[dateStr] = newAvailability;

				// If apply to all weekdays of the same type is checked
				if (applyToWeekday) {
					const date = new Date(dateStr);
					const dayOfWeek = date.getDay();
					const year = date.getFullYear();
					const month = date.getMonth();
					const lastDay = new Date(year, month + 1, 0).getDate();

					// Loop through all days in the month
					for (let i = 1; i <= lastDay; i++) {
						const currDate = new Date(year, month, i);
						// If same day of week
						if (currDate.getDay() === dayOfWeek) {
							const currDateStr = `${year}-${String(
								month + 1
							).padStart(2, "0")}-${String(i).padStart(2, "0")}`;
							// Only update if not the same as the original date
							if (currDateStr !== dateStr) {
								doctorAvailability[currDateStr] = {
									...newAvailability,
								};
							}
						}
					}
				}

				// Update calendar
				updateCalendar();

				// Show success message
				showAlert("Availability updated successfully", "success");

				document.body.removeChild(modal);
			});
	}

	// Show modal to create new appointment from calendar
	function showNewAppointmentModal(dateString) {
		const dateObj = new Date(dateString);
		const formattedDate = dateObj.toLocaleDateString("en-US", {
			weekday: "long",
			month: "long",
			day: "numeric",
			year: "numeric",
		});

		const modal = document.createElement("div");
		modal.className =
			"fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50";
		modal.innerHTML = `
                <div class="bg-white rounded-lg p-6 max-w-md w-full">
                    <div class="flex justify-between items-center mb-4">
                        <h3 class="text-lg font-semibold text-gray-800">New Appointment for ${formattedDate}</h3>
                        <button class="close-modal text-gray-500 hover:text-gray-700">
                            <i class="ri-close-line"></i>
                        </button>
                    </div>
                    <div class="space-y-4">
                        <div>
                            <label class="block text-sm font-medium text-gray-700 mb-1">Patient</label>
                            <select id="new-appointment-patient" class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-primary focus:border-primary">
                                ${patients
									.map(
										(patient) => `
                                    <option value="${patient.id}">${patient.name} (${patient.id})</option>
                                `
									)
									.join("")}
                            </select>
                        </div>
                        <div>
                            <label class="block text-sm font-medium text-gray-700 mb-1">Time</label>
                            <select id="new-appointment-time" class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-primary focus:border-primary">
                                <option value="08:00">8:00 AM</option>
                                <option value="09:00">9:00 AM</option>
                                <option value="10:00">10:00 AM</option>
                                <option value="11:00">11:00 AM</option>
                                <option value="13:00">1:00 PM</option>
                                <option value="14:00">2:00 PM</option>
                                <option value="15:00">3:00 PM</option>
                                <option value="16:00">4:00 PM</option>
                            </select>
                        </div>
                        <div>
                            <label class="block text-sm font-medium text-gray-700 mb-1">Purpose</label>
                            <input type="text" id="new-appointment-purpose" class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-primary focus:border-primary" placeholder="e.g. Routine Checkup">
                        </div>
                    </div>
                    <div class="flex justify-end space-x-4 mt-6">
                        <button class="close-modal px-4 py-2 border border-gray-300 rounded-button text-gray-700 hover:bg-gray-50">
                            Cancel
                        </button>
                        <button id="create-appointment" class="px-4 py-2 bg-primary text-white rounded-button hover:bg-primary-dark">
                            Create Appointment
                        </button>
                    </div>
                </div>
            `;

		document.body.appendChild(modal);

		// Close modal event
		modal.querySelectorAll(".close-modal").forEach((button) => {
			button.addEventListener("click", function () {
				document.body.removeChild(modal);
			});
		});

		// Create appointment event
		document
			.getElementById("create-appointment")
			.addEventListener("click", function () {
				const patientId = document.getElementById(
					"new-appointment-patient"
				).value;
				const time = document.getElementById(
					"new-appointment-time"
				).value;
				const purpose = document
					.getElementById("new-appointment-purpose")
					.value.trim();

				if (!patientId || !purpose) {
					showAlert("Please fill in all required fields", "error");
					return;
				}

				// Get patient info
				const patient = patients.find((p) => p.id === patientId);
				if (!patient) {
					showAlert("Invalid patient selected", "error");
					return;
				}

				// Format time (add AM/PM)
				const timeParts = time.split(":");
				let hours = parseInt(timeParts[0]);
				const minutes = timeParts[1];
				const ampm = hours >= 12 ? "PM" : "AM";
				hours = hours % 12;
				hours = hours ? hours : 12; // the hour '0' should be '12'
				const formattedTime = `${hours}:${minutes} ${ampm}`;

				// Generate new appointment ID
				const newId = `APT-${String(appointments.length + 1).padStart(
					3,
					"0"
				)}`;

				// Create new appointment
				const newAppointment = {
					id: newId,
					date: dateString,
					time: formattedTime,
					patientId: patient.id,
					patientName: patient.name,
					purpose: purpose,
					status: "confirmed",
				};

				// Add to appointments array
				appointments.push(newAppointment);

				// Re-render calendar
				updateCalendar();

				// Show success alert
				showAlert("Appointment created successfully", "success");

				document.body.removeChild(modal);
			});
	}

	// Update today's appointments section
	function updateTodayAppointments() {
		const todayDate = new Date(
			currentYear,
			currentMonth,
			currentDate.getDate()
		)
			.toISOString()
			.split("T")[0];
		const todayAppts = appointments.filter((apt) => apt.date === todayDate);

		const todayAppointmentsSection =
			document.getElementById("today-appointments");
		const todayAppointmentsList = document.getElementById(
			"today-appointments-list"
		);

		// Show or hide section based on if there are appointments
		if (todayAppts.length > 0) {
			todayAppointmentsSection.classList.remove("hidden");

			// Clear and rebuild list
			todayAppointmentsList.innerHTML = "";

			// Sort by time
			todayAppts.sort((a, b) => {
				const timeA = new Date(
					`2000-01-01T${a.time.replace(/am|pm/i, "")}`
				).getTime();
				const timeB = new Date(
					`2000-01-01T${b.time.replace(/am|pm/i, "")}`
				).getTime();
				return timeA - timeB;
			});

			// Add each appointment
			todayAppts.forEach((apt) => {
				todayAppointmentsList.innerHTML += `
                        <div class="flex items-center p-4 border border-gray-200 rounded-md hover:bg-gray-50">
                            <div class="flex-shrink-0">
                                <div class="flex h-12 w-12 items-center justify-center rounded-full bg-primary-50 text-primary">
                                    <i class="ri-calendar-check-line text-xl"></i>
                                </div>
                            </div>
                            <div class="ml-4 flex-grow">
                                <div class="flex justify-between">
                                    <div>
                                        <p class="text-sm font-medium text-gray-900">${
											apt.time
										}</p>
                                        <p class="text-xs text-gray-500">${
											apt.purpose
										}</p>
                                    </div>
                                    <div class="text-right">
                                        <p class="text-sm font-medium text-gray-900">${
											apt.patientName
										}</p>
                                        <p class="text-xs text-gray-500">ID: ${
											apt.patientId
										}</p>
                                    </div>
                                </div>
                            </div>
                            <div class="ml-4">
                                <span class="px-2 inline-flex text-xs leading-5 font-semibold rounded-full ${
									apt.status === "confirmed"
										? "bg-green-100 text-green-800"
										: apt.status === "pending"
										? "bg-yellow-100 text-yellow-800"
										: "bg-gray-100 text-gray-800"
								}">${apt.status}</span>
                            </div>
                        </div>
                    `;
			});
		} else {
			todayAppointmentsSection.classList.add("hidden");
		}
	}

	// Calendar navigation
	prevMonthButton.addEventListener("click", function () {
		currentMonth--;
		if (currentMonth < 0) {
			currentMonth = 11;
			currentYear--;
		}
		updateCalendar();
	});

	nextMonthButton.addEventListener("click", function () {
		currentMonth++;
		if (currentMonth > 11) {
			currentMonth = 0;
			currentYear++;
		}
		updateCalendar();
	});

	// Initialize calendar
	updateCalendar();
}

// Reviews section
async function setupReviewsSection() {
	const reviews = await GetAllReviewsForDoctorById(window.User.id,token)
    
    console.log(reviews)

	const reviewsPerPage = 3; // Now you can change this to any number!
	let currentPage = 1;
	let currentFilter = "all";

	const reviewsContainer = document.getElementById("reviews-container");
	const filterSelect = document.getElementById("review-filter");
	const pageNumbersContainer = document.getElementById("page-numbers");
	const prevBtn = document.getElementById("prev-btn");
	const nextBtn = document.getElementById("next-btn");
	const prevMobile = document.getElementById("prev-mobile");
	const nextMobile = document.getElementById("next-mobile");
	const showingRange = document.getElementById("showing-range-lg");
	const totalResults = document.getElementById("total-reviews"); // ← FIXED ID

	function renderStars(rating) {
		return Array.from(
			{ length: 5 },
			(_, i) => `<i class="ri-star${i < rating ? "-fill" : "-line"}"></i>`
		).join("");
	}

	function getFilteredReviews() {
		return currentFilter === "all"
			? reviews
			: reviews.filter((r) => r.rating === parseInt(currentFilter));
	}

	function renderReviews() {
		const filtered = getFilteredReviews();
		const start = (currentPage - 1) * reviewsPerPage;
		const end = start + reviewsPerPage;
		const pageReviews = filtered.slice(start, end);

		reviewsContainer.innerHTML = pageReviews
			.map(
				(review) => `
                <div class="review-item border-b border-gray-200 pb-6" data-rating="${
					review.rating
				}">
                    <div class="flex justify-between items-start">
                        <div class="flex items-start">
                            <div class="ml-4">
                                <div class="flex text-yellow-400 mt-1">${renderStars(
									review.rating
								)}</div>
                            </div>
                        </div>
                        <span class="text-sm text-gray-500">${
							review.reviewDate
						}</span>
                    </div>
                    <p class="mt-3 text-gray-700">${review.reviewText}</p>
                </div>
            `
			)
			.join("");

		updateReviewRange(currentPage, reviewsPerPage, filtered.length);
		renderPaginationButtons(filtered.length);
	}

	function renderPaginationButtons(totalFiltered) {
		const totalPages = Math.ceil(totalFiltered / reviewsPerPage);
		pageNumbersContainer.innerHTML = "";

		for (let i = 1; i <= totalPages; i++) {
			const btn = document.createElement("button");
			btn.textContent = i;
			btn.className = `relative inline-flex items-center px-4 py-2 border text-sm font-medium ${
				i === currentPage
					? "bg-primary text-white"
					: "bg-white text-gray-700"
			} border-gray-300 hover:bg-gray-50`;
			btn.addEventListener("click", () => {
				currentPage = i;
				renderReviews();
			});
			pageNumbersContainer.appendChild(btn);
		}

		const disablePrev = currentPage === 1;
		const disableNext = currentPage === totalPages || totalPages === 0;

		prevBtn.disabled = disablePrev;
		nextBtn.disabled = disableNext;
		prevMobile.disabled = disablePrev;
		nextMobile.disabled = disableNext;
	}

	function updateReviewRange(currentPage, pageSize, totalReviews) {
		const start = (currentPage - 1) * pageSize + 1;
		const end = Math.min(currentPage * pageSize, totalReviews);
		showingRange.textContent = `${start}–${end}`;
		totalResults.textContent = totalReviews;
	}

	filterSelect.addEventListener("change", () => {
		currentFilter = filterSelect.value;
		currentPage = 1;
		renderReviews();
	});

	[prevBtn, prevMobile].forEach((btn) =>
		btn.addEventListener("click", () => {
			if (currentPage > 1) {
				currentPage--;
				renderReviews();
			}
		})
	);

	[nextBtn, nextMobile].forEach((btn) =>
		btn.addEventListener("click", () => {
			const total = getFilteredReviews().length;
			if (currentPage < Math.ceil(total / reviewsPerPage)) {
				currentPage++;
				renderReviews();
			}
		})
	);

	renderReviews();
}

// Patient Table Functionality
async function initPatientTable() {
	const patients = await getDoctorPatients(window.User.id, token);
    console.log(patients)
	let currentSearch = localStorage.getItem("search") || "";
	let filters = {
		gender: localStorage.getItem("gender") || "",
		date: localStorage.getItem("date") || "",
	};
	let currentSort = {
		column: localStorage.getItem("sortColumn") || "",
		direction: localStorage.getItem("sortDirection") || "",
	};
	let currentPage = parseInt(localStorage.getItem("currentPage")) || 1;
	const rowsPerPage = 5;

	const tbody = document.querySelector("#patients-tbody");
	const searchInput = document.querySelector("#patient-search-input");
	const genderSelect = document.querySelector("#patient-gender-filter");
	const dateInput = document.querySelector("#patient-date-filter");
	const clearBtn = document.querySelector("#clear-patient-filters");
	const showingRange = document.querySelectorAll("#patient-showing-range");
	const totalCount = document.querySelectorAll("#patient-total-count");
	const sortButtons = document.querySelectorAll(".sort-btn");
	const prevBtn = document.querySelector("#patient-prev-btn");
	const nextBtn = document.querySelector("#patient-next-btn");
	const pageNumbers = document.querySelector("#patient-page-numbers");

	function applyFilters(data) {
		return data
			.filter((p) => {
				return (
					p.patientName
						.toLowerCase()
						.includes(currentSearch.toLowerCase())
				);
			})
	}

	function applySort(data) {
		if (!currentSort.column) return data;

		return [...data].sort((a, b) => {
			if (a[currentSort.column] < b[currentSort.column])
				return currentSort.direction === "asc" ? -1 : 1;
			if (a[currentSort.column] > b[currentSort.column])
				return currentSort.direction === "asc" ? 1 : -1;
			return 0;
		});
	}

	function paginate(data) {
		const start = (currentPage - 1) * rowsPerPage;
		return data.slice(start, start + rowsPerPage);
	}

	function renderTable() {
		let filtered = applyFilters(patients);
		let sorted = applySort(filtered);
		let paginated = paginate(sorted);

		tbody.innerHTML = "";
		paginated.forEach((p) => {
			tbody.innerHTML += `
                    <tr>
                        <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900 flex items-center gap-2">
                            ${p.patientName}
                        </td>
                        <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">${p.patientId}</td>
                        <td class="px-6 py-4 whitespace-nowrap text-sm cursor-pointer flex gap-2">
                            <button class="view-patient-button" data-id="${p.patientId}">
                                <i class="ri-eye-line text-primary"></i>
                            </button>
                            <button class="edit-patient-button" data-id="${p.patientId}">
                                <i class="ri-pencil-line text-primary"></i>
                            </button>
                            <button class="delete-patient-button" data-id="${p.patientId}">
                                <i class="ri-delete-bin-line text-red-500"></i>
                            </button>
                        </td>
                    </tr>
                `;
		});

		// Add event listeners to edit buttons AFTER rendering
		document.querySelectorAll(".edit-patient-button").forEach((button) => {
			button.addEventListener("click", () => {
				const patientId = button.getAttribute("data-id");
				const patient = patients.find((p) => p.patientId === patientId);
				if (!patient) return alert("Patient not found!");

				// Fill modal inputs with patient data
				const modal = document.getElementById("edit-patient-modal");
				modal.style.display = "flex";

				const form = modal.querySelector("#edit-patient-form");
				form.id.value = patient.patientId;
				form.name.value = patient.patientName;

				// Enable confirm button only if condition input changes
				const conditionInput = form.condition;
				const confirmBtn = form.querySelector("#confirm-btn");
				confirmBtn.disabled = true;

				conditionInput.addEventListener("input", () => {
					confirmBtn.disabled =
						conditionInput.value.trim() ===
						patient.condition.trim();
				});

				// Confirm button action
				confirmBtn.onclick = () => {
					patient.condition = conditionInput.value.trim();
					confirmBtn.disabled = true;
					// Show success message
					const msg = form.querySelector("#confirmation-message");
					msg.classList.remove("hidden");
					// Update table or re-render as needed
					renderTable();
					// Hide message after 2 seconds and close modal
					setTimeout(() => {
						msg.classList.add("hidden");
						modal.style.display = "none";
					}, 2000);
				};

				// Cancel button action
				const cancelBtn = form.querySelector("#cancel-btn");
				cancelBtn.onclick = () => {
					modal.style.display = "none";
				};
			});
		});

		const total = filtered.length;
		const start = (currentPage - 1) * rowsPerPage + 1;
		const end = Math.min(start + rowsPerPage - 1, total);
		showingRange.forEach((el) => (el.textContent = `${start}-${end}`));
		totalCount.forEach((el) => (el.textContent = total));

		// Pagination buttons
		renderPagination(Math.ceil(total / rowsPerPage));
	}

	function renderPagination(totalPages) {
		pageNumbers.innerHTML = "";
		for (let i = 1; i <= totalPages; i++) {
			const btn = document.createElement("button");
			btn.textContent = i;
			btn.className = `px-3 py-1 border text-sm ${
				i === currentPage
					? "bg-primary text-white"
					: "bg-white text-gray-700"
			} hover:bg-gray-100`;
			btn.addEventListener("click", () => {
				currentPage = i;
				localStorage.setItem("currentPage", currentPage);
				renderTable();
			});
			pageNumbers.appendChild(btn);
		}

		prevBtn.disabled = currentPage === 1;
		nextBtn.disabled = currentPage === totalPages;
	}

	// Event Listeners
	searchInput.value = currentSearch;
	searchInput.addEventListener("input", (e) => {
		currentSearch = e.target.value;
		localStorage.setItem("search", currentSearch);
		currentPage = 1;
		renderTable();
	});

	genderSelect.value = filters.gender;
	genderSelect.addEventListener("change", (e) => {
		filters.gender = e.target.value;
		localStorage.setItem("gender", filters.gender);
		currentPage = 1;
		renderTable();
	});

	dateInput.value = filters.date;
	dateInput.addEventListener("change", (e) => {
		filters.date = e.target.value;
		localStorage.setItem("date", filters.date);
		currentPage = 1;
		renderTable();
	});

	clearBtn.addEventListener("click", () => {
		filters = { gender: "", date: "" };
		localStorage.removeItem("gender");
		localStorage.removeItem("date");
		genderSelect.value = "";
		dateInput.value = "";
		renderTable();
	});

	sortButtons.forEach((btn) => {
		btn.addEventListener("click", () => {
			const col = btn.dataset.column;
			if (currentSort.column === col) {
				currentSort.direction =
					currentSort.direction === "asc" ? "desc" : "asc";
			} else {
				currentSort.column = col;
				currentSort.direction = "asc";
			}
			localStorage.setItem("sortColumn", currentSort.column);
			localStorage.setItem("sortDirection", currentSort.direction);
			renderTable();
		});
	});

	prevBtn.addEventListener("click", () => {
		if (currentPage > 1) {
			currentPage--;
			localStorage.setItem("currentPage", currentPage);
			renderTable();
		}
	});

	nextBtn.addEventListener("click", () => {
		currentPage++;
		localStorage.setItem("currentPage", currentPage);
		renderTable();
	});

	// Initial render
	renderTable();
}

// Setup Update Availability button
function setupUpdateAvailabilityButton() {
	const updateAvailabilityBtn = document.getElementById(
		"main-update-availability"
	);
	if (!updateAvailabilityBtn) return;

	updateAvailabilityBtn.addEventListener("click", function () {
		// Get today's date
		const today = new Date();

		// For demo purposes, use a date where we have availability data (April 15, 2025)
		const demoDate = new Date(2025, 3, 15);
		const dateString = demoDate.toISOString().split("T")[0];

		// Get availability for this date
		const availability = doctorAvailability[dateString] || {
			morning: false,
			afternoon: false,
			evening: false,
		};

		// Show update availability modal
		showUpdateAvailabilityModal(dateString, availability);
	});
}

// Setup Prescription Management
function setupPrescriptionManagement() {
	// Sample prescriptions data
	const prescriptions = [
		{
			id: "PRE-001",
			date: "2025-04-15",
			patientId: "MDF-P001",
			patientName: "Milla Willow",
			patientImage:
				"../assets/images/Admin pages/Doctors page/Milla Willow_Admin.jpg",
			medication: "Lisinopril 10mg",
			instructions: "Take once daily in the morning",
			status: "active",
			statusClass: "bg-green-100 text-green-800",
		},
		{
			id: "PRE-002",
			date: "2025-04-10",
			patientId: "MDF-P002",
			patientName: "John Smith",
			patientImage: "../assets/images/patients/john-smith.jpg",
			medication: "Metformin 500mg",
			instructions: "Take twice daily with meals",
			status: "active",
			statusClass: "bg-green-100 text-green-800",
		},
		{
			id: "PRE-003",
			date: "2025-03-28",
			patientId: "MDF-P003",
			patientName: "Emma Johnson",
			patientImage: "../assets/images/patients/emma-johnson.jpg",
			medication: "Albuterol Inhaler",
			instructions: "Use as needed for asthma symptoms",
			status: "active",
			statusClass: "bg-green-100 text-green-800",
		},
	];

	// Render prescriptions
	function renderPrescriptions() {
		const prescriptionsTable = document.querySelector(
			"#prescriptions tbody"
		);
		if (!prescriptionsTable) return;

		prescriptionsTable.innerHTML = "";

		prescriptions.forEach((prescription) => {
			prescriptionsTable.innerHTML += `
                        <tr class="hover:bg-gray-50">
                            <td class="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">${prescription.date}</td>
                            <td class="px-6 py-4 whitespace-nowrap">
                                <div class="flex items-center">
                                    <div class="flex-shrink-0 h-10 w-10">
                                        <img class="h-10 w-10 rounded-full object-cover" src="${prescription.patientImage}" alt="${prescription.patientName}">
                                    </div>
                                    <div class="ml-4">
                                        <div class="text-sm font-medium text-gray-900">${prescription.patientName}</div>
                                        <div class="text-sm text-gray-500">ID: ${prescription.patientId}</div>
                                    </div>
                                </div>
                            </td>
                            <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">${prescription.medication}</td>
                            <td class="px-6 py-4 whitespace-nowrap">
                                <span class="px-2 inline-flex text-xs leading-5 font-semibold rounded-full ${prescription.statusClass}">${prescription.status}</span>
                            </td>
                            <td class="px-6 py-4 whitespace-nowrap text-sm font-medium">
                                <a href="#" class="text-primary hover:text-primary-dark mr-3 view-prescription" data-prescription-id="${prescription.id}"><i class="ri-eye-line"></i></a>
                                <a href="#" class="text-teal-500 hover:text-blue-700 mr-3 edit-prescription" data-prescription-id="${prescription.id}"><i class="ri-edit-line"></i></a>
                                <a href="#" class="text-red-500 hover:text-red-700 print-prescription" data-prescription-id="${prescription.id}"><i class="ri-printer-line"></i></a>
                            </td>
                        </tr>
                    `;
		});

		// Add event listeners to view buttons
		document.querySelectorAll(".view-prescription").forEach((button) => {
			button.addEventListener("click", function (e) {
				e.preventDefault();
				const prescriptionId = this.dataset.prescriptionId;
				viewPrescription(prescriptionId);
			});
		});

		// Add event listeners to edit buttons
		document.querySelectorAll(".edit-prescription").forEach((button) => {
			button.addEventListener("click", function (e) {
				e.preventDefault();
				const prescriptionId = this.dataset.prescriptionId;
				editPrescription(prescriptionId);
			});
		});

		// Add event listeners to print buttons
		document.querySelectorAll(".print-prescription").forEach((button) => {
			button.addEventListener("click", function (e) {
				e.preventDefault();
				const prescriptionId = this.dataset.prescriptionId;
				printPrescription(prescriptionId);
			});
		});
	}

	// View prescription details
	function viewPrescription(prescriptionId) {
		const prescription = prescriptions.find((p) => p.id === prescriptionId);
		if (!prescription) return;

		const modal = document.createElement("div");
		modal.className =
			"fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50";
		modal.innerHTML = `
                    <div class="bg-white rounded-lg p-6 max-w-md w-full">
                        <div class="flex justify-between items-center mb-4">
                            <h3 class="text-lg font-semibold text-gray-800">Prescription Details</h3>
                            <button class="close-modal text-gray-500 hover:text-gray-700">
                                <i class="ri-close-line"></i>
                            </button>
                        </div>
                        <div class="space-y-4">
                            <div>
                                <label class="block text-sm font-medium text-gray-700 mb-1">Patient</label>
                                <div class="flex items-center">
                                    <div class="flex-shrink-0 h-10 w-10">
                                        <img class="h-10 w-10 rounded-full object-cover" src="${prescription.patientImage}" alt="${prescription.patientName}">
                                    </div>
                                    <div class="ml-3">
                                        <div class="text-sm font-medium text-gray-900">${prescription.patientName}</div>
                                        <div class="text-sm text-gray-500">ID: ${prescription.patientId}</div>
                                    </div>
                                </div>
                            </div>
                            <div>
                                <label class="block text-sm font-medium text-gray-700 mb-1">Date</label>
                                <p class="text-sm text-gray-900">${prescription.date}</p>
                            </div>
                            <div>
                                <label class="block text-sm font-medium text-gray-700 mb-1">Medication</label>
                                <p class="text-sm text-gray-900">${prescription.medication}</p>
                            </div>
                            <div>
                                <label class="block text-sm font-medium text-gray-700 mb-1">Instructions</label>
                                <p class="text-sm text-gray-900">${prescription.instructions}</p>
                            </div>
                            <div>
                                <label class="block text-sm font-medium text-gray-700 mb-1">Status</label>
                                <span class="px-2 inline-flex text-xs leading-5 font-semibold rounded-full ${prescription.statusClass}">${prescription.status}</span>
                            </div>
                        </div>
                        <div class="flex justify-end space-x-4 mt-6">
                            <button class="close-modal px-4 py-2 border border-gray-300 rounded-button text-gray-700 hover:bg-gray-50">
                                Close
                            </button>
                        </div>
                    </div>
                `;

		document.body.appendChild(modal);

		// Close modal event
		modal.querySelectorAll(".close-modal").forEach((button) => {
			button.addEventListener("click", function () {
				document.body.removeChild(modal);
			});
		});
	}

	// Edit prescription
	function editPrescription(prescriptionId) {
		const prescription = prescriptions.find((p) => p.id === prescriptionId);
		if (!prescription) return;

		const modal = document.createElement("div");
		modal.className =
			"fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50";
		modal.innerHTML = `
                    <div class="bg-white rounded-lg p-6 max-w-md w-full">
                        <div class="flex justify-between items-center mb-4">
                            <h3 class="text-lg font-semibold text-gray-800">Edit Prescription</h3>
                            <button class="close-modal text-gray-500 hover:text-gray-700">
                                <i class="ri-close-line"></i>
                            </button>
                        </div>
                        <div class="space-y-4">
                            <div>
                                <label class="block text-sm font-medium text-gray-700 mb-1">Patient</label>
                                <div class="flex items-center">
                                    <div class="flex-shrink-0 h-10 w-10">
                                        <img class="h-10 w-10 rounded-full object-cover" src="${
											prescription.patientImage
										}" alt="${prescription.patientName}">
                                    </div>
                                    <div class="ml-3">
                                        <div class="text-sm font-medium text-gray-900">${
											prescription.patientName
										}</div>
                                        <div class="text-sm text-gray-500">ID: ${
											prescription.patientId
										}</div>
                                    </div>
                                </div>
                            </div>
                            <div>
                                <label class="block text-sm font-medium text-gray-700 mb-1">Date</label>
                                <input type="date" id="edit-prescription-date" class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-primary focus:border-primary" value="${
									prescription.date
								}">
                            </div>
                            <div>
                                <label class="block text-sm font-medium text-gray-700 mb-1">Medication</label>
                                <input type="text" id="edit-prescription-medication" class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-primary focus:border-primary" value="${
									prescription.medication
								}">
                            </div>
                            <div>
                                <label class="block text-sm font-medium text-gray-700 mb-1">Instructions</label>
                                <textarea id="edit-prescription-instructions" class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-primary focus:border-primary">${
									prescription.instructions
								}</textarea>
                            </div>
                            <div>
                                <label class="block text-sm font-medium text-gray-700 mb-1">Status</label>
                                <select id="edit-prescription-status" class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-primary focus:border-primary">
                                    <option value="active" ${
										prescription.status === "active"
											? "selected"
											: ""
									}>Active</option>
                                    <option value="expired" ${
										prescription.status === "expired"
											? "selected"
											: ""
									}>Expired</option>
                                    <option value="discontinued" ${
										prescription.status === "discontinued"
											? "selected"
											: ""
									}>Discontinued</option>
                                </select>
                            </div>
                        </div>
                        <div class="flex justify-end space-x-4 mt-6">
                            <button class="close-modal px-4 py-2 border border-gray-300 rounded-button text-gray-700 hover:bg-gray-50">
                                Cancel
                            </button>
                            <button id="save-prescription" class="px-4 py-2 bg-primary text-white rounded-button hover:bg-primary-dark">
                                Save Changes
                            </button>
                        </div>
                    </div>
                `;

		document.body.appendChild(modal);

		// Close modal event
		modal.querySelectorAll(".close-modal").forEach((button) => {
			button.addEventListener("click", function () {
				document.body.removeChild(modal);
			});
		});

		// Save prescription event
		document
			.getElementById("save-prescription")
			.addEventListener("click", function () {
				const date = document.getElementById(
					"edit-prescription-date"
				).value;
				const medication = document
					.getElementById("edit-prescription-medication")
					.value.trim();
				const instructions = document
					.getElementById("edit-prescription-instructions")
					.value.trim();
				const status = document.getElementById(
					"edit-prescription-status"
				).value;

				if (!date || !medication || !instructions) {
					showAlert("Please fill in all required fields", "error");
					return;
				}

				// Update prescription
				prescription.date = date;
				prescription.medication = medication;
				prescription.instructions = instructions;
				prescription.status = status;

				// Update status class
				let statusClass = "";
				switch (status) {
					case "active":
						statusClass = "bg-green-100 text-green-800";
						break;
					case "expired":
						statusClass = "bg-gray-100 text-gray-800";
						break;
					case "discontinued":
						statusClass = "bg-red-100 text-red-800";
						break;
				}
				prescription.statusClass = statusClass;

				// Re-render prescriptions
				renderPrescriptions();

				// Show success message
				showAlert("Prescription updated successfully", "success");

				document.body.removeChild(modal);
			});
	}

	// New prescription button
	const newPrescriptionBtn = document.querySelector("#prescriptions button");
	if (newPrescriptionBtn) {
		newPrescriptionBtn.addEventListener("click", function () {
			const modal = document.createElement("div");
			modal.className =
				"fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50";
			modal.innerHTML = `
                        <div class="bg-white rounded-lg p-6 max-w-md w-full">
                            <div class="flex justify-between items-center mb-4">
                                <h3 class="text-lg font-semibold text-gray-800">New Prescription</h3>
                                <button class="close-modal text-gray-500 hover:text-gray-700">
                                    <i class="ri-close-line"></i>
                                </button>
                            </div>
                            <div class="space-y-4">
                                <div>
                                    <label class="block text-sm font-medium text-gray-700 mb-1">Patient</label>
                                    <select id="new-prescription-patient" class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-primary focus:border-primary">
                                        ${patients
											.map(
												(patient) => `
                                            <option value="${patient.id}">${patient.name} (${patient.id})</option>
                                        `
											)
											.join("")}
                                    </select>
                                </div>
                                <div>
                                    <label class="block text-sm font-medium text-gray-700 mb-1">Date</label>
                                    <input type="date" id="new-prescription-date" class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-primary focus:border-primary">
                                </div>
                                <div>
                                    <label class="block text-sm font-medium text-gray-700 mb-1">Medication</label>
                                    <input type="text" id="new-prescription-medication" class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-primary focus:border-primary" placeholder="e.g. Lisinopril 10mg">
                                </div>
                                <div>
                                    <label class="block text-sm font-medium text-gray-700 mb-1">Instructions</label>
                                    <textarea id="new-prescription-instructions" class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-primary focus:border-primary" placeholder="e.g. Take once daily with food"></textarea>
                                </div>
                                <div>
                                    <label class="block text-sm font-medium text-gray-700 mb-1">Status</label>
                                    <select id="new-prescription-status" class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-primary focus:border-primary">
                                        <option value="active" selected>Active</option>
                                        <option value="expired">Expired</option>
                                        <option value="discontinued">Discontinued</option>
                                    </select>
                                </div>
                            </div>
                            <div class="flex justify-end space-x-4 mt-6">
                                <button class="close-modal px-4 py-2 border border-gray-300 rounded-button text-gray-700 hover:bg-gray-50">
                                    Cancel
                                </button>
                                <button id="create-prescription" class="px-4 py-2 bg-primary text-white rounded-button hover:bg-primary-dark">
                                    Create Prescription
                                </button>
                            </div>
                        </div>
                    `;

			document.body.appendChild(modal);

			// Set default date to today
			const today = new Date();
			document.getElementById("new-prescription-date").value = today
				.toISOString()
				.split("T")[0];

			// Close modal event
			modal.querySelectorAll(".close-modal").forEach((button) => {
				button.addEventListener("click", function () {
					document.body.removeChild(modal);
				});
			});

			// Create prescription event
			document
				.getElementById("create-prescription")
				.addEventListener("click", function () {
					const patientId = document.getElementById(
						"new-prescription-patient"
					).value;
					const date = document.getElementById(
						"new-prescription-date"
					).value;
					const medication = document
						.getElementById("new-prescription-medication")
						.value.trim();
					const instructions = document
						.getElementById("new-prescription-instructions")
						.value.trim();
					const status = document.getElementById(
						"new-prescription-status"
					).value;

					if (!patientId || !date || !medication || !instructions) {
						showAlert(
							"Please fill in all required fields",
							"error"
						);
						return;
					}

					// Get patient info
					const patient = patients.find((p) => p.id === patientId);
					if (!patient) {
						showAlert("Invalid patient selected", "error");
						return;
					}

					// Generate new prescription ID
					const newId = `PRE-${String(
						prescriptions.length + 1
					).padStart(3, "0")}`;

					// Create status class
					let statusClass = "";
					switch (status) {
						case "active":
							statusClass = "bg-green-100 text-green-800";
							break;
						case "expired":
							statusClass = "bg-gray-100 text-gray-800";
							break;
						case "discontinued":
							statusClass = "bg-red-100 text-red-800";
							break;
					}

					// Create new prescription
					const newPrescription = {
						id: newId,
						date: date,
						patientId: patient.id,
						patientName: patient.name,
						patientImage: patient.image,
						medication: medication,
						instructions: instructions,
						status: status,
						statusClass: statusClass,
					};

					// Add to prescriptions array
					prescriptions.push(newPrescription);

					// Re-render prescriptions
					renderPrescriptions();

					// Show success alert
					showAlert("Prescription created successfully", "success");

					document.body.removeChild(modal);
				});
		});
	}

	// Initialize prescriptions
	renderPrescriptions();
}

// Initialize all functionality
document.addEventListener("DOMContentLoaded", function () {
	// Core UI components
	setupProfileTabs();
	setupMobileMenu();
	setupDesktopDropdowns();
	setupScrollbars();

	// Data and functionality
	setupAppointmentManagement();
	setupCalendar();
	initPatientTable();
	// filterPatients();
	// setupPagination();
	// initPagination();
	setupUpdateAvailabilityButton();
	setupPrescriptionManagement();

	// UI components that depend on data
	setupReviewsSection();
});

const toggleFilterBtn = document.getElementById("toggle-advanced-filter");
const advancedFilterSection = document.getElementById(
	"advanced-filter-section"
);
const toggleIcon = document.getElementById("toggle-icon");

toggleFilterBtn.addEventListener("click", () => {
	if (advancedFilterSection.classList.contains("hidden")) {
		advancedFilterSection.classList.remove("hidden");
		toggleIcon.classList.add("rotate-180");
	} else {
		advancedFilterSection.classList.add("hidden");
		toggleIcon.classList.remove("rotate-180");
	}
});
const initializeHTML = (doctor) => {
	const template = `<div class="card bg-white p-4 rounded-lg shadow-md w-full col-span-2">
            <div class="card-title text-2xl font-bold mb-4 text-primary border-b-2 border-blue-300 pb-2">
                <h2>
                    <i class="ri-user-3-line ri-1x text-primary"></i>
                    Doctor Information
                </h2>
            </div>

            <div class="grid grid-cols-1 gap-6 items-start">
                <!-- Above: Photo, ID, Name -->
                <div class="flex flex-col items-center">
                    <div class="doctor-photo w-24 h-24 rounded-full overflow-hidden mb-4 border-4 border-blue-100 shadow">
                        <img src="../assets/images/Dr. Sarah Johnson.jpg" 
                            alt="Doctor Profile" 
                            class="w-full h-full object-cover">
                    </div>
                    <div class="doctor-NameId text-center">
                        <p class="text-sm text-gray-500 flex items-center gap-1 justify-center">
                            <i data-feather="hash" class="w-4 h-4 text-teal-500"></i> DOC-C001
                        </p>
                        <p class="text-2xl font-bold text-primary mt-1">Dr. ${doctor.doctorName}</p>
                        <p class="text-sm text-gray-600 mt-1">${doctor.specialty}</p>  
                    </div>
                </div>

                <!-- Below: Details with Icons -->
                <div class="grid grid-cols-1 gap-6 p-2">
                    <!-- Row 1: Specialization, Experience, Department -->
                    <div class="grid grid-cols-1 sm:grid-cols-2 gap-4">
                        <div class="flex flex-col">
                            <p class="text-sm text-gray-500 flex items-center gap-1">
                                <i data-feather="award" class="w-4 h-4 text-teal-500"></i> Specialization
                            </p>
                            <p class="font-semibold text-gray-800">${doctor.specialty}</p>
                        </div>
                        <div class="flex flex-col">
                            <p class="text-sm text-gray-500 flex items-center gap-1">
                                <i data-feather="briefcase" class="w-4 h-4 text-teal-500"></i> Experience
                            </p>
                            <p class="font-semibold text-gray-800"> + ${doctor.yearsOfExperience}</p>
                        </div>
                    </div>

                    <!-- Row 2: Phone, Email, Address -->
                    <div class="grid grid-cols-1 sm:grid-cols-2 gap-4">
                        <div class="flex flex-col">
                            <p class="text-sm text-gray-500 flex items-center gap-1">
                                <i data-feather="phone" class="w-4 h-4 text-teal-500"></i> Phone
                            </p>
                            <p class="font-semibold text-gray-800">${doctor.contactInfo}</p>
                        </div>
                        <div class="flex flex-col">
                            <p class="text-sm text-gray-500 flex items-center gap-1">
                                <i data-feather="mail" class="w-4 h-4 text-teal-500"></i> Email
                            </p>
                            <p class="font-semibold text-gray-800 break-words">${doctor.email}</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>`;

	return template;
};
const renderDoctorCard = async () => {
	const claims = window.User;
	const doctor = await getDoctorById(claims.id, token);

	const HTML = initializeHTML(doctor);

	const container = document.getElementById("doctorContainer");

	container.innerHTML = HTML;
};
renderDoctorCard();
