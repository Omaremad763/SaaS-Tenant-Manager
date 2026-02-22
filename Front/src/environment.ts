const isLocal = window.location.hostname === 'localhost';

export const environment = {
  production: !isLocal,
  apiUrl: isLocal ? 'https://localhost:44380/api' : 'server url',
};
