FileUploadSample
----------------

This sample illustrates how to upload files to an ApiController using HttpClient
using MIME Multipart File Upload as defined by HTML. It also shows how to set up 
progress notifications with HttpClient using ProgressNotificationHandler.

The FileUploadController reads the contents of an HTML file upload asynchronously and writes one 
or more body parts to a local file. It then responds with a result containing information about
the uploaded file (or files).
