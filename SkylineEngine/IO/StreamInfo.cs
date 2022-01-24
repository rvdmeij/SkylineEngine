namespace SkylineEngine.IO
{
    public struct StreamInfo
    {
        public string filename;
        public long fileSize;
        public long chunkSize;
        public long totalChunks;
        public long lastChunkSize;

        public StreamInfo(string filename, long fileSize, long chunkSize = 1024)
        {
            this.filename = filename;
            this.fileSize = fileSize;
            this.chunkSize = chunkSize;

            totalChunks = fileSize / chunkSize;
            lastChunkSize = fileSize % chunkSize;

            if (lastChunkSize != 0) /* if the above division was uneven */
            {
                ++totalChunks; /* add an unfilled final chunk */
            }
            else /* if division was even, last chunk is full */
            {
                lastChunkSize = chunkSize;
            }
        }
    }
}
