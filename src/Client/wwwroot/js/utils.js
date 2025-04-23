// Download file
window.downloadFile = (filename, contentType, content) => {
   // Create a blob with the data
   const blob = new Blob([content], { type: contentType });

   // Create a link element
   const link = document.createElement('a');
   link.href = window.URL.createObjectURL(blob);
   link.download = filename;

   // Append to the document, click it, and remove it
   document.body.appendChild(link);
   link.click();
   document.body.removeChild(link);
};