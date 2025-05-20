import { getDoctorPatients } from "../../assets/js/api/doctors";
import {getToken} from '../../assets/js/utils/jwt'

function getUserRoleFromToken() {
    if (window.User) {
        return window.User.role;
    }
} 
function getUserIdFromToken() {
    if (window.User) {
        return window.User.id;
    }
}


const userRole = getUserRoleFromToken(); 
const currentUserId = getUserIdFromToken();

// SignalR Connection
const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7227/chatHub", {
        accessTokenFactory: () => localStorage.getItem("token")
    })
    .withAutomaticReconnect()
    .build();

// Store available users to chat with
let availableUsers = [];

const chatToggle = document.getElementById("chat-toggle");
const chatContainer = document.getElementById("chat-container");
const chatClose = document.getElementById("chat-close");
const messageInput = document.getElementById("message-input");
const sendButton = document.getElementById("send-button");
const chatMessages = document.getElementById("chat-messages");
const userSelector = document.getElementById("user-selector"); 

// Toggle chat visibility
chatToggle.addEventListener("click", () => {
    chatContainer.classList.toggle("active");
    if (chatContainer.classList.contains("active")) {
        loadAvailableUsers();
    }
});

// Load users available for chatting
async function loadAvailableUsers() {
    // Call your API to get users based on role
    // Example: Doctors see patients, Patients see doctors
    const response = await getDoctorPatients(currentUserId,getToken())
    availableUsers = await response.json();
    
    // Populate dropdown
    userSelector.innerHTML = '';
    availableUsers.forEach(user => {
        const option = document.createElement("option");
        option.value = user.id;
        option.textContent = user.name;
        userSelector.appendChild(option);
    });
}

// Send message to selected user
function sendMessage() {
    const message = messageInput.value.trim();
    const recipientId = userSelector.value;
    
    if (message && recipientId) {
        connection.invoke("SendMessageToUser", recipientId, message)
            .then(() => {
                addMessage("You", message, "sent");
                messageInput.value = "";
            })
            .catch(err => console.error("Send failed:", err));
    }
}

// Receive messages
connection.on("ReceivePrivateMessage", (senderId, message) => {
    const sender = availableUsers.find(u => u.id === senderId)?.name || "Unknown";
    addMessage(sender, message, "received");
});

// Start connection
connection.start()
    .then(() => {
        console.log("Connected to chat hub");
        connection.invoke("RegisterUser", userRole);
    })
    .catch(err => console.error("Connection failed:", err));