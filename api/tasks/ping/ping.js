export const usePingTask = (httpClient) => {
    return {
        start: async (url) => {
            const metadata = { 
                startTime: new Date(),
                endTime: 0
            };
            const response = await httpClient.get(url)
            metadata.endTime = new Date()
            response.duration = metadata.endTime - metadata.startTime
            return response
        }
    }
}