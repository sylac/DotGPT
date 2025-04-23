// Focus on an element
window.focusElement = (element) => {
   if (element) {
      element.focus();
   }
};

// Scroll to the bottom of an element
window.scrollToBottom = (elementId) => {
   const element = document.getElementById(elementId);
   if (element) {
      element.scrollTop = element.scrollHeight;
   }
};