const clientId = import.meta.env.VITE_AZURE_AD_CLIENT_ID;
const authority = import.meta.env.VITE_AZURE_AD_AUTHORITY;
const tenantId = import.meta.env.VITE_AZURE_AD_TENANT_ID;

export const msalConfig = {
  auth: {
    clientId,
    authority: authority ?? `https://login.microsoftonline.com/${tenantId}`,
    redirectUri: window.location.origin
  },
  cache: {
    cacheLocation: 'localStorage',
    storeAuthStateInCookie: false
  }
};

export const loginRequest = {
  scopes: ['api://00000000-0000-0000-0000-000000000000/Feedback.ReadWrite']
};
