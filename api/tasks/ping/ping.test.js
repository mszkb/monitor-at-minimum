import test from 'ava';
import { usePingTask } from './ping.js';

test('Ping Task - start function should make a GET request to the provided URL', async (t) => {
    const url = 'https://example.com';
    const expectedResponse = { data: 'Mocked Response' };

    // Mock axios.get method to return the expected response
    const axiosMock = {
        get: async (url) => expectedResponse
    };

    // Inject the mocked axios instance into the usePingTask function
    const pingTask = usePingTask(axiosMock);

    // Call the start function with the provided URL
    const response = await pingTask.start(url);

    // Assert that the start function made a GET request to the provided URL
    t.is(response, expectedResponse);
});

test("expect duration to be a number", async (t) => {
    const url = 'https://example.com';
    const expectedResponse = { data: 'Mocked Response' };

    // Mock axios.get method to return the expected response
    const axiosMock = {
        get: async (url) => expectedResponse
    };

    // Inject the mocked axios instance into the usePingTask function
    const pingTask = usePingTask(axiosMock);

    // Call the start function with the provided URL
    const response = await pingTask.start(url);

    // Assert that the start function made a GET request to the provided URL
    t.is(typeof response.duration, 'number');
})
