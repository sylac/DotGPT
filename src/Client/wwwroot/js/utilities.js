/**
 * Downloads a file with the specified name, content type, and content
 * @param {string} fileName - The name of the file to download
 * @param {string} contentType - The mime type of the file
 * @param {string} content - The file content
 */
function downloadFile(fileName, contentType, content) {
   // Create a blob with the data
   const blob = new Blob([content], { type: contentType });

   // Create a link element
   const link = document.createElement('a');

   // Set link properties
   link.href = window.URL.createObjectURL(blob);
   link.download = fileName;

   // Append to the document
   document.body.appendChild(link);

   // Trigger the download
   link.click();

   // Clean up
   document.body.removeChild(link);
}