import axios from 'axios';

const apiBaseUrl = import.meta.env.VITE_API_BASE_URL ?? '/api';

export const createClient = (accessToken) => {
  const client = axios.create({
    baseURL: apiBaseUrl,
    headers: {
      'Content-Type': 'application/json'
    }
  });

  client.interceptors.request.use((config) => {
    if (accessToken) {
      config.headers.Authorization = `Bearer ${accessToken}`;
    }
    return config;
  });

  return client;
};
