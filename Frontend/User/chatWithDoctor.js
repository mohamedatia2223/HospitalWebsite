class DoctorChat {
  constructor() {
    this.conversations = JSON.parse(localStorage.getItem('doctorConversations')) || {};
    this.currentDoctorId = null;
    this.isMaximized = false;
    this.mediaRecorder = null;
    this.audioChunks = [];
    this.createChatContainer();
    this.initElements();
    this.bindEvents();
    this.loadDoctors();
  }

  createChatContainer() {
    this.chatContainer = document.createElement('div');
    this.chatContainer.id = 'doctor-chat-container';
    this.chatContainer.className = 'hidden fixed bottom-20 right-6 w-[800px] h-[600px] bg-white rounded-lg shadow-xl flex z-50';
    this.chatContainer.innerHTML = `
      <!-- Doctors List - Left Sidebar -->
      <div class="w-1/3 border-r border-gray-200 flex flex-col">
        <div class="bg-teal-500 text-white p-4 rounded-tl-lg">
          <h3 class="font-semibold">Available Doctors</h3>
        </div>
        <div class="flex-1 overflow-y-auto" id="doctors-list"></div>
      </div>
      
      <!-- Chat Interface - Right Side -->
      <div class="w-2/3 flex flex-col">
        <!-- Chat Header -->
        <div class="bg-gray-50 p-3 border-b border-gray-200 flex items-center justify-between">
          <div class="flex items-center">
            <div class="w-10 h-10 rounded-full bg-teal-100 flex items-center justify-center mr-3">
              <i class="ri-user-3-line text-teal-600" id="current-doctor-icon"></i>
            </div>
            <div>
              <p class="font-medium" id="current-doctor-name">Select a doctor</p>
              <p class="text-xs text-gray-500" id="current-doctor-specialty"></p>
            </div>
          </div>
          <div class="flex space-x-2">
            <button id="maximize-chat-btn" class="p-2 text-gray-600 hover:text-teal-600 rounded-full hover:bg-gray-100">
              <i class="ri-fullscreen-line"></i>
            </button>
            <button id="minimize-chat-btn" class="p-2 text-gray-600 hover:text-teal-600 rounded-full hover:bg-gray-100 hidden">
              <i class="ri-fullscreen-exit-line"></i>
            </button>
            <button id="audio-call-btn" class="p-2 text-gray-600 hover:text-teal-600 rounded-full hover:bg-gray-100">
              <i class="ri-phone-line"></i>
            </button>
            <button id="video-call-btn" class="p-2 text-gray-600 hover:text-teal-600 rounded-full hover:bg-gray-100">
              <i class="ri-vidicon-line"></i>
            </button>
            <button id="close-chat-btn" class="p-2 text-gray-600 hover:text-teal-600 rounded-full hover:bg-gray-100">
              <i class="ri-close-line"></i>
            </button>
          </div>
        </div>
        
        <!-- Chat Messages -->
        <div id="chat-messages" class="flex-1 overflow-y-auto p-4 space-y-3 bg-gray-50"></div>
        
        <!-- Message Input Area -->
        <div class="border-t border-gray-200 p-3 bg-white">
          <!-- File Upload Button -->
          <div class="flex items-center mb-2">
            <button id="attach-file-btn" class="p-2 text-gray-500 hover:text-teal-600 rounded-full hover:bg-gray-100 mr-1">
              <i class="ri-attachment-2"></i>
              <input type="file" id="file-input" class="hidden" multiple>
            </button>
            <button id="record-audio-btn" class="p-2 text-gray-500 hover:text-teal-600 rounded-full hover:bg-gray-100 mr-1">
              <i class="ri-mic-line"></i>
            </button>
          </div>
          
          <!-- Message Input -->
          <div class="flex">
            <input type="text" id="chat-message-input" 
                  class="flex-1 border rounded-l-lg p-3 text-sm focus:ring-2 focus:ring-teal-500 focus:border-teal-500" 
                  placeholder="Type a message...">
            <button id="send-message-btn" class="bg-teal-500 text-white px-4 rounded-r-lg hover:bg-teal-600">
              <i class="ri-send-plane-line"></i>
            </button>
          </div>
        </div>
      </div>
      
      <!-- Audio Recording Modal -->
      <div id="audio-modal" class="hidden fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-[60]">
        <div class="bg-white rounded-lg p-6 w-80">
          <div class="flex justify-between items-center mb-4">
            <h3 class="text-lg font-semibold">Recording Audio</h3>
            <div class="flex items-center space-x-2">
              <span id="recording-time">00:00</span>
              <div class="w-3 h-3 rounded-full bg-red-500 animate-pulse"></div>
            </div>
          </div>
          <div class="flex justify-center space-x-4">
            <button id="stop-recording-btn" class="px-4 py-2 bg-red-500 text-white rounded-lg hover:bg-red-600">
              <i class="ri-stop-circle-line mr-2"></i>Stop
            </button>
            <button id="cancel-recording-btn" class="px-4 py-2 border border-gray-300 rounded-lg hover:bg-gray-100">
              Cancel
            </button>
          </div>
        </div>
      </div>
      
      <!-- Message Options Modal -->
      <div id="message-options-modal" class="hidden fixed bg-white shadow-lg rounded-md p-1 z-[70]">
        <button class="delete-for-me w-full text-left px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 rounded">
          <i class="ri-delete-bin-line mr-2"></i>Delete for me
        </button>
        <button class="delete-for-all w-full text-left px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 rounded">
          <i class="ri-delete-bin-fill mr-2"></i>Delete for everyone
        </button>
      </div>
    `;
    document.body.appendChild(this.chatContainer);
  }

  initElements() {
    this.elements = {
      chatBtn: document.getElementById('doctor-chat-btn'),
      chatContainer: this.chatContainer,
      closeBtn: document.getElementById('close-chat-btn'),
      maximizeBtn: document.getElementById('maximize-chat-btn'),
      minimizeBtn: document.getElementById('minimize-chat-btn'),
      audioCallBtn: document.getElementById('audio-call-btn'),
      videoCallBtn: document.getElementById('video-call-btn'),
      doctorsList: document.getElementById('doctors-list'),
      chatMessages: document.getElementById('chat-messages'),
      messageInput: document.getElementById('chat-message-input'),
      sendBtn: document.getElementById('send-message-btn'),
      attachFileBtn: document.getElementById('attach-file-btn'),
      fileInput: document.getElementById('file-input'),
      recordAudioBtn: document.getElementById('record-audio-btn'),
      audioModal: document.getElementById('audio-modal'),
      stopRecordingBtn: document.getElementById('stop-recording-btn'),
      cancelRecordingBtn: document.getElementById('cancel-recording-btn'),
      recordingTime: document.getElementById('recording-time'),
      doctorName: document.getElementById('current-doctor-name'),
      doctorSpecialty: document.getElementById('current-doctor-specialty'),
      doctorIcon: document.getElementById('current-doctor-icon'),
      messageOptionsModal: document.getElementById('message-options-modal')
    };
  }

  bindEvents() {
    // Basic chat functionality
    this.elements.chatBtn.addEventListener('click', () => this.toggleChat());
    this.elements.closeBtn.addEventListener('click', () => this.closeChat());
    this.elements.maximizeBtn.addEventListener('click', () => this.toggleMaximize());
    this.elements.minimizeBtn.addEventListener('click', () => this.toggleMaximize());
    this.elements.sendBtn.addEventListener('click', () => this.sendMessage());
    this.elements.messageInput.addEventListener('keypress', (e) => {
      if (e.key === 'Enter') this.sendMessage();
    });

    // File attachment
    this.elements.attachFileBtn.addEventListener('click', () => this.elements.fileInput.click());
    this.elements.fileInput.addEventListener('change', (e) => this.handleFileUpload(e.target.files));

    // Audio recording
    this.elements.recordAudioBtn.addEventListener('click', () => this.startAudioRecording());
    this.elements.stopRecordingBtn.addEventListener('click', () => this.stopAudioRecording(true));
    this.elements.cancelRecordingBtn.addEventListener('click', () => this.stopAudioRecording(false));

    // Message options
    document.addEventListener('click', (e) => {
      if (!e.target.closest('#message-options-modal') && !e.target.closest('.message-options-btn')) {
        this.elements.messageOptionsModal.classList.add('hidden');
      }
    });

    this.elements.messageOptionsModal.querySelector('.delete-for-me').addEventListener('click', () => {
      this.deleteMessage(this.selectedMessageId, false);
      this.elements.messageOptionsModal.classList.add('hidden');
    });

    this.elements.messageOptionsModal.querySelector('.delete-for-all').addEventListener('click', () => {
      this.deleteMessage(this.selectedMessageId, true);
      this.elements.messageOptionsModal.classList.add('hidden');
    });
  }

  toggleMaximize() {
    this.isMaximized = !this.isMaximized;
    
    if (this.isMaximized) {
      this.chatContainer.classList.remove('bottom-20', 'right-6', 'w-[800px]', 'h-[600px]');
      this.chatContainer.classList.add('inset-0', 'rounded-none');
      this.elements.maximizeBtn.classList.add('hidden');
      this.elements.minimizeBtn.classList.remove('hidden');
    } else {
      this.chatContainer.classList.add('bottom-20', 'right-6', 'w-[800px]', 'h-[600px]');
      this.chatContainer.classList.remove('inset-0', 'rounded-none');
      this.elements.minimizeBtn.classList.add('hidden');
      this.elements.maximizeBtn.classList.remove('hidden');
    }
  }

  handleFileUpload(files) {
    if (!this.currentDoctorId || !files || files.length === 0) return;
    
    Array.from(files).forEach(file => {
      const reader = new FileReader();
      reader.onload = (e) => {
        const timestamp = new Date().toISOString();
        const fileType = file.type.split('/')[0];
        
        const fileData = {
          id: 'file_' + Date.now(),
          sender: 'patient',
          type: fileType,
          name: file.name,
          size: file.size,
          data: e.target.result,
          timestamp: timestamp,
          status: 'sent'
        };
        
        this.saveMessage(fileData);
        this.displayFileMessage(fileData);
        
        // Simulate doctor response
        setTimeout(() => {
          const responses = {
            'image': "Thanks for sharing the image. I'll review it.",
            'video': "I've received the video clip.",
            'audio': "I'll listen to your recording shortly.",
            'application': "I've received the document."
          };
          
          this.saveMessage({
            id: 'msg_' + Date.now(),
            sender: 'doctor',
            type: 'text',
            content: responses[fileType] || "I've received your file.",
            timestamp: new Date().toISOString()
          });
        }, 2000);
      };
      
      if (file.type.includes('image')) {
        reader.readAsDataURL(file);
      } else {
        reader.readAsBinaryString(file);
      }
    });
    
    this.elements.fileInput.value = '';
  }

  saveMessage(message) {
    if (!this.currentDoctorId) return;
    
    if (!this.conversations[this.currentDoctorId]) {
      this.conversations[this.currentDoctorId] = [];
    }
    
    this.conversations[this.currentDoctorId].push(message);
    this.saveConversations();
    this.displayMessages();
  }

  displayFileMessage(fileData) {
    const messageDiv = document.createElement('div');
    messageDiv.className = `flex ${fileData.sender === 'patient' ? 'justify-end' : 'justify-start'} mb-3`;
    messageDiv.dataset.messageId = fileData.id;
    
    let contentHTML = '';
    if (fileData.type === 'image' && fileData.data) {
      contentHTML = `
        <div class="max-w-xs rounded-lg overflow-hidden border border-gray-200 bg-white relative">
          <img src="${fileData.data}" alt="Attached image" class="w-full h-auto">
          <div class="p-2">
            <p class="text-sm truncate">${fileData.name}</p>
            <p class="text-xs text-gray-500">${this.formatFileSize(fileData.size)}</p>
          </div>
          ${fileData.sender === 'patient' ? this.getMessageOptionsButton(fileData.id) : ''}
        </div>
      `;
    } else {
      contentHTML = `
        <div class="${fileData.sender === 'patient' ? 'bg-teal-100' : 'bg-gray-100'} rounded-lg p-3 max-w-xs relative">
          <div class="flex items-center">
            <i class="ri-file-${this.getFileTypeIcon(fileData.type)}-line text-teal-600 mr-2"></i>
            <div>
              <p class="text-sm font-medium">${fileData.name}</p>
              <p class="text-xs text-gray-500">${this.formatFileSize(fileData.size)}</p>
            </div>
          </div>
          ${fileData.sender === 'patient' ? this.getMessageOptionsButton(fileData.id) : ''}
        </div>
      `;
    }
    
    messageDiv.innerHTML = contentHTML;
    this.elements.chatMessages.appendChild(messageDiv);
    this.elements.chatMessages.scrollTop = this.elements.chatMessages.scrollHeight;
  }

  getMessageOptionsButton(messageId) {
    return `
      <button class="message-options-btn absolute top-1 right-1 w-6 h-6 flex items-center justify-center rounded-full bg-white bg-opacity-70 text-gray-500 hover:text-teal-600 hover:bg-opacity-100">
        <i class="ri-more-2-fill text-sm"></i>
      </button>
    `;
  }

  async startAudioRecording() {
    try {
      this.elements.audioModal.classList.remove('hidden');
      this.audioChunks = [];
      this.recordingStartTime = new Date();
      this.updateRecordingTime();
      
      const stream = await navigator.mediaDevices.getUserMedia({ audio: true });
      this.mediaRecorder = new MediaRecorder(stream);
      
      this.mediaRecorder.ondataavailable = (e) => {
        if (e.data.size > 0) {
          this.audioChunks.push(e.data);
        }
      };
      
      this.mediaRecorder.start(100); // Collect data every 100ms
      
      // Add event listeners to buttons
      this.elements.stopRecordingBtn.onclick = () => this.finishAudioRecording(true);
      this.elements.cancelRecordingBtn.onclick = () => this.finishAudioRecording(false);
    } catch (error) {
      console.error('Error starting recording:', error);
      alert('Could not access microphone. Please check permissions.');
      this.stopAudioRecording(false);
    }
  }

  finishAudioRecording(saveRecording) {
    clearTimeout(this.recordingTimer);
    this.elements.audioModal.classList.add('hidden');
    
    if (saveRecording && this.audioChunks.length > 0) {
      const audioBlob = new Blob(this.audioChunks, { type: 'audio/webm' });
      const audioUrl = URL.createObjectURL(audioBlob);
      const duration = Math.floor((new Date() - this.recordingStartTime) / 1000);
      
      const audioData = {
        id: 'audio_' + Date.now(),
        sender: 'patient',
        type: 'audio',
        duration: duration,
        url: audioUrl,
        timestamp: new Date().toISOString(),
        status: 'sent'
      };
      
      this.saveMessage(audioData);
      
      // Simulate doctor response
      setTimeout(() => {
        this.saveMessage({
          id: 'msg_' + Date.now(),
          sender: 'doctor',
          type: 'text',
          content: "I'll listen to your recording shortly.",
          timestamp: new Date().toISOString()
        });
      }, 2000);
    }
    
    if (this.mediaRecorder && this.mediaRecorder.state !== 'inactive') {
      this.mediaRecorder.stop();
    }
    
    if (this.mediaRecorder && this.mediaRecorder.stream) {
      this.mediaRecorder.stream.getTracks().forEach(track => track.stop());
    }
    
    this.recordingStartTime = null;
    this.audioChunks = [];
    this.mediaRecorder = null;
  }

  stopAudioRecording(saveRecording) {
    this.finishAudioRecording(saveRecording);
  }

  displayAudioMessage(audioData) {
    const messageDiv = document.createElement('div');
    messageDiv.className = `flex ${audioData.sender === 'patient' ? 'justify-end' : 'justify-start'} mb-3`;
    messageDiv.dataset.messageId = audioData.id;
    
    messageDiv.innerHTML = `
      <div class="${audioData.sender === 'patient' ? 'bg-teal-100' : 'bg-gray-100'} rounded-lg p-3 max-w-xs relative">
        <div class="flex items-center">
          <button class="play-audio-btn w-10 h-10 rounded-full bg-teal-500 text-white flex items-center justify-center mr-3">
            <i class="ri-play-line"></i>
          </button>
          <div>
            <p class="text-sm font-medium">Audio Message</p>
            <p class="text-xs text-gray-500">${audioData.duration} seconds</p>
          </div>
        </div>
        <audio src="${audioData.url}" class="hidden"></audio>
        ${audioData.sender === 'patient' ? this.getMessageOptionsButton(audioData.id) : ''}
      </div>
    `;
    
    // Add play button functionality
    messageDiv.querySelector('.play-audio-btn').addEventListener('click', (e) => {
      const audio = messageDiv.querySelector('audio');
      if (audio.paused) {
        audio.play();
        e.target.innerHTML = '<i class="ri-pause-line"></i>';
        audio.onended = () => {
          e.target.innerHTML = '<i class="ri-play-line"></i>';
        };
      } else {
        audio.pause();
        audio.currentTime = 0;
        e.target.innerHTML = '<i class="ri-play-line"></i>';
      }
    });
    
    // Add click handler for options button
    const optionsBtn = messageDiv.querySelector('.message-options-btn');
    if (optionsBtn) {
      optionsBtn.addEventListener('click', (e) => {
        e.stopPropagation();
        this.selectedMessageId = audioData.id;
        this.showMessageOptions(e.target);
      });
    }
    
    this.elements.chatMessages.appendChild(messageDiv);
    this.elements.chatMessages.scrollTop = this.elements.chatMessages.scrollHeight;
  }

  showMessageOptions(target) {
    const rect = target.getBoundingClientRect();
    this.elements.messageOptionsModal.style.top = `${rect.bottom + window.scrollY}px`;
    this.elements.messageOptionsModal.style.left = `${rect.left + window.scrollX - 100}px`;
    this.elements.messageOptionsModal.classList.remove('hidden');
  }

  deleteMessage(messageId, deleteForAll) {
    if (!this.currentDoctorId || !this.conversations[this.currentDoctorId]) return;
    
    const messageIndex = this.conversations[this.currentDoctorId].findIndex(m => m.id === messageId);
    if (messageIndex === -1) return;
    
    if (deleteForAll) {
      // In a real app, you would notify the server to delete for both parties
      this.conversations[this.currentDoctorId].splice(messageIndex, 1);
    } else {
      // Just mark as deleted for this user
      this.conversations[this.currentDoctorId][messageIndex].status = 'deleted';
    }
    
    this.saveConversations();
    this.displayMessages();
  }

  // ... [rest of your existing methods]
}

document.addEventListener('DOMContentLoaded', () => {
  new DoctorChat();
});