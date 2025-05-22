// Admin Dashboard JavaScript

document.addEventListener('DOMContentLoaded', () => {
    // Mobile Menu Toggle
    const mobileMenuBtn = document.getElementById('mobile-menu-button');
    const mobileMenu = document.getElementById('mobile-menu');
    
    mobileMenuBtn.addEventListener('click', () => {
        mobileMenu.classList.toggle('active');
    });

    // Close mobile menu when clicking outside
    document.addEventListener('click', (e) => {
        if (!mobileMenuBtn.contains(e.target) && !mobileMenu.contains(e.target)) {
            mobileMenu.classList.remove('active');
        }
    });

    // Search Functionality
    const searchInput = document.getElementById('search-input');
    const searchResults = document.getElementById('search-results');
    
    searchInput.addEventListener('input', (e) => {
        const searchTerm = e.target.value.toLowerCase();
        if (searchTerm.length >= 2) {
            // Simulate API call to search
            searchPatientsAndDoctors(searchTerm);
        } else {
            searchResults.innerHTML = '';
        }
    });

    // Notification System
    const notificationBtn = document.getElementById('notification-btn');
    const notifications = [
        { id: 1, title: 'New Appointment', message: 'Patient John Doe has scheduled an appointment', time: '10 mins ago' },
        { id: 2, title: 'Doctor Request', message: 'Dr. Smith has requested a new shift', time: '2 hours ago' },
        { id: 3, title: 'System Update', message: 'System maintenance scheduled for tonight', time: '5 hours ago' }
    ];

    notificationBtn.addEventListener('click', () => {
        // Show notifications modal
        const modal = document.createElement('div');
        modal.className = 'fixed inset-0 bg-gray-500 bg-opacity-50 flex items-center justify-center';
        modal.innerHTML = `
            <div class="bg-white rounded-lg p-6 max-w-md w-full">
                <h2 class="text-xl font-bold mb-4">Notifications</h2>
                <div class="space-y-4">
                    ${notifications.map(notification => `
                        <div class="p-4 border rounded-lg">
                            <h3 class="font-semibold">${notification.title}</h3>
                            <p class="text-gray-600">${notification.message}</p>
                            <p class="text-sm text-gray-500">${notification.time}</p>
                        </div>
                    `).join('')}
                </div>
                <button onclick="this.closest('.fixed').remove()" class="mt-4 px-4 py-2 bg-primary text-white rounded hover:bg-primary/90">
                    Close
                </button>
            </div>
        `;
        document.body.appendChild(modal);
    });

    // Profile Dropdown
    const profileBtn = document.getElementById('profile-btn');
    const profileMenu = document.createElement('div');
    profileMenu.className = 'hidden absolute right-0 mt-2 w-48 bg-white rounded-md shadow-lg py-1';
    profileMenu.innerHTML = `
        <a href="#" class="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100">Profile</a>
        <a href="#" class="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100">Settings</a>
        <a href="#" class="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100">Logout</a>
    `;
    profileBtn.appendChild(profileMenu);

    profileBtn.addEventListener('click', (e) => {
        e.preventDefault();
        profileMenu.classList.toggle('hidden');
    });

    // Quick Actions
    const quickActions = document.querySelectorAll('.quick-action');
    quickActions.forEach(action => {
        action.addEventListener('click', () => {
            const actionType = action.dataset.action;
            handleQuickAction(actionType);
        });
    });


});

function searchPatientsAndDoctors(query) {
    // Simulated search function
    const results = [
        { type: 'patient', name: 'John Doe', id: 'P123' },
        { type: 'doctor', name: 'Dr. Smith', id: 'D456' },
        { type: 'patient', name: 'Jane Smith', id: 'P789' }
    ];

    const searchResults = document.getElementById('search-results');
    searchResults.innerHTML = `
        <div class="bg-white rounded-lg shadow p-4">
            ${results.map(result => `
                <div class="p-3 border-b last:border-b-0">
                    <span class="font-medium">${result.name}</span>
                    <span class="text-sm text-gray-500">${result.type}</span>
                </div>
            `).join('')}
        </div>
    `;
}

function handleQuickAction(actionType) {
    switch(actionType) {
        case 'add-patient':
            openPatientForm();
            break;
        case 'add-doctor':
            openDoctorForm();
            break;
        case 'schedule-appointment':
            openAppointmentModal();
            break;
        case 'generate-report':
            generateReport();
            break;
        default:
            console.log('Unknown action:', actionType);
    }
}

function showAlert(type, message) {
    const alertTypes = {
        success: 'bg-green-100 text-green-700 border-green-400',
        warning: 'bg-yellow-100 text-yellow-700 border-yellow-400',
        error: 'bg-red-100 text-red-700 border-red-400'
    };

    const alertDiv = document.createElement('div');
    alertDiv.className = `fixed top-4 right-4 p-4 rounded-lg border ${alertTypes[type]}`;
    alertDiv.innerHTML = `
        <div class="flex items-center">
            <i class="ri-${type === 'success' ? 'check-circle' : type === 'warning' ? 'alert' : 'close-circle'}-fill text-xl mr-2"></i>
            <span>${message}</span>
        </div>
    `;
    
    document.body.appendChild(alertDiv);
    
    setTimeout(() => {
        alertDiv.remove();
    }, 5000);
}

function generateReport() {
    const reportType = prompt('Select report type:\n1. PDF\n2. DOC\n3. Statistics');
    
    if (!reportType) return;

    const statistics = {
        totalPatients: 150,
        totalDoctors: 30,
        appointmentsToday: 25,
        pendingAppointments: 45,
        completedAppointments: 180
    };

    let content = '';
    
    if (reportType === '1' || reportType === '2') {
        content = `
            <h1 class="text-2xl font-bold mb-4">Hospital Report</h1>
            <div class="space-y-4">
                <div>
                    <span class="font-semibold">Total Patients:</span> ${statistics.totalPatients}
                </div>
                <div>
                    <span class="font-semibold">Total Doctors:</span> ${statistics.totalDoctors}
                </div>
                <div>
                    <span class="font-semibold">Appointments Today:</span> ${statistics.appointmentsToday}
                </div>
                <div>
                    <span class="font-semibold">Pending Appointments:</span> ${statistics.pendingAppointments}
                </div>
                <div>
                    <span class="font-semibold">Completed Appointments:</span> ${statistics.completedAppointments}
                </div>
            </div>
        `;
    }

    if (reportType === '1') {
        // Generate PDF
        const doc = new jsPDF();
        doc.html(content, {
            callback: function (doc) {
                doc.save('hospital-report.pdf');
            }
        });
    } else if (reportType === '2') {
        // Generate DOC
        const blob = new Blob([content], { type: 'application/msword' });
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = 'hospital-report.doc';
        a.click();
        window.URL.revokeObjectURL(url);
    } else if (reportType === '3') {
        // Show statistics
        showAlert('success', 'Statistics report generated successfully');
        console.log('Statistics:', statistics);
    }
}

function initializeCharts() {
    // Appointments Chart
    const appointmentsCtx = document.getElementById('appointmentsChart');
    if (appointmentsCtx) {
        new Chart(appointmentsCtx, {
            type: 'line',
            data: {
                labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun'],
                datasets: [{
                    label: 'Appointments',
                    data: [12, 19, 3, 5, 2, 3],
                    borderColor: '#1CB5BD',
                    tension: 0.4
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    legend: {
                        position: 'top',
                    },
                },
                scales: {
                    y: {
                        beginAtZero: true
                    }
                }
            }
        });
    }

    // Other charts can be initialized here
}

function openPatientForm() {
    const modal = document.createElement('div');
    modal.className = 'fixed inset-0 bg-gray-500 bg-opacity-50 flex items-center justify-center';
    modal.innerHTML = `
        <div class="bg-white rounded-lg p-6 max-w-md w-full">
            <h2 class="text-xl font-bold mb-4">Add New Patient</h2>
            <form class="space-y-4" onsubmit="event.preventDefault(); handlePatientSubmit(this)">
                <div>
                    <label class="block text-sm font-medium text-gray-700">Photo</label>
                    <input type="file" accept="image/*" class="mt-1 block w-full rounded-md border-gray-300 shadow-sm">
                </div>
                <div>
                    <label class="block text-sm font-medium text-gray-700">Full Name</label>
                    <input type="text" name="name" class="mt-1 block w-full rounded-md border-gray-300 shadow-sm" required>
                </div>
                <div>
                    <label class="block text-sm font-medium text-gray-700">Email</label>
                    <input type="email" name="email" class="mt-1 block w-full rounded-md border-gray-300 shadow-sm" required>
                </div>
                <div>
                    <label class="block text-sm font-medium text-gray-700">Phone</label>
                    <input type="tel" name="phone" class="mt-1 block w-full rounded-md border-gray-300 shadow-sm" required>
                </div>
                <div>
                    <label class="block text-sm font-medium text-gray-700">Date of Birth</label>
                    <input type="date" name="dob" class="mt-1 block w-full rounded-md border-gray-300 shadow-sm" required>
                </div>
                <div class="flex justify-end space-x-2">
                    <button type="button" onclick="this.closest('.fixed').remove()" class="px-4 py-2 bg-gray-200 text-gray-700 rounded hover:bg-gray-300">
                        Cancel
                    </button>
                    <button type="submit" class="px-4 py-2 bg-primary text-white rounded hover:bg-primary/90">
                        Save
                    </button>
                </div>
            </form>
        </div>
    `;
    document.body.appendChild(modal);
}

function openDoctorForm() {
    const modal = document.createElement('div');
    modal.className = 'fixed inset-0 bg-gray-500 bg-opacity-50 flex items-center justify-center';
    modal.innerHTML = `
        <div class="bg-white rounded-lg p-6 max-w-md w-full">
            <h2 class="text-xl font-bold mb-4">Add New Doctor</h2>
            <form class="space-y-4" onsubmit="event.preventDefault(); handleDoctorSubmit(this)">
                <div>
                    <label class="block text-sm font-medium text-gray-700">Photo</label>
                    <input type="file" accept="image/*" class="mt-1 block w-full rounded-md border-gray-300 shadow-sm">
                </div>
                <div>
                    <label class="block text-sm font-medium text-gray-700">Full Name</label>
                    <input type="text" name="name" class="mt-1 block w-full rounded-md border-gray-300 shadow-sm" required>
                </div>
                <div>
                    <label class="block text-sm font-medium text-gray-700">Specialization</label>
                    <input type="text" name="specialization" class="mt-1 block w-full rounded-md border-gray-300 shadow-sm" required>
                </div>
                <div>
                    <label class="block text-sm font-medium text-gray-700">Email</label>
                    <input type="email" name="email" class="mt-1 block w-full rounded-md border-gray-300 shadow-sm" required>
                </div>
                <div>
                    <label class="block text-sm font-medium text-gray-700">Phone</label>
                    <input type="tel" name="phone" class="mt-1 block w-full rounded-md border-gray-300 shadow-sm" required>
                </div>
                <div class="flex justify-end space-x-2">
                    <button type="button" onclick="this.closest('.fixed').remove()" class="px-4 py-2 bg-gray-200 text-gray-700 rounded hover:bg-gray-300">
                        Cancel
                    </button>
                    <button type="submit" class="px-4 py-2 bg-primary text-white rounded hover:bg-primary/90">
                        Save
                    </button>
                </div>
            </form>
        </div>
    `;
    document.body.appendChild(modal);
}

function openAppointmentModal() {
    const modal = document.createElement('div');
    modal.className = 'fixed inset-0 bg-gray-500 bg-opacity-50 flex items-center justify-center';
    modal.innerHTML = `
        <div class="bg-white rounded-lg p-6 max-w-md w-full">
            <h2 class="text-xl font-bold mb-4">Schedule Appointment</h2>
            <form class="space-y-4" onsubmit="event.preventDefault(); handleAppointmentSubmit(this)">
                <div>
                    <label class="block text-sm font-medium text-gray-700">Patient</label>
                    <select name="patient" class="mt-1 block w-full rounded-md border-gray-300 shadow-sm">
                        <option value="">Select Patient</option>
                        <option value="P1001">John Doe</option>
                        <option value="P1002">Jane Smith</option>
                        <option value="P1003">Bob Johnson</option>
                    </select>
                </div>
                <div>
                    <label class="block text-sm font-medium text-gray-700">Doctor</label>
                    <select name="doctor" class="mt-1 block w-full rounded-md border-gray-300 shadow-sm">
                        <option value="">Select Doctor</option>
                        <option value="D1001">Dr. Smith</option>
                        <option value="D1002">Dr. Johnson</option>
                        <option value="D1003">Dr. Williams</option>
                    </select>
                </div>
                <div>
                    <label class="block text-sm font-medium text-gray-700">Date</label>
                    <input type="date" name="date" class="mt-1 block w-full rounded-md border-gray-300 shadow-sm" required>
                </div>
                <div>
                    <label class="block text-sm font-medium text-gray-700">Time</label>
                    <input type="time" name="time" class="mt-1 block w-full rounded-md border-gray-300 shadow-sm" required>
                </div>
                <div>
                    <label class="block text-sm font-medium text-gray-700">Reason</label>
                    <textarea name="reason" class="mt-1 block w-full rounded-md border-gray-300 shadow-sm" rows="3"></textarea>
                </div>
                <div class="flex justify-end space-x-2">
                    <button type="button" onclick="this.closest('.fixed').remove()" class="px-4 py-2 bg-gray-200 text-gray-700 rounded hover:bg-gray-300">
                        Cancel
                    </button>
                    <button type="submit" class="px-4 py-2 bg-primary text-white rounded hover:bg-primary/90">
                        Schedule
                    </button>
                </div>
            </form>
        </div>
    `;
    document.body.appendChild(modal);
}

function openDoctorForm() {
    // Similar to openPatientForm but for doctors
    console.log('Opening doctor form');
}

function openAppointmentForm() {
    // Similar to openPatientForm but for appointments
    console.log('Opening appointment form');
}
