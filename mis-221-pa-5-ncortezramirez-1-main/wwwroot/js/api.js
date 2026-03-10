const API_BASE = window.location.origin + '/api';

async function apiCall(endpoint, method = 'GET', body = null) {
  try {
    const options = {
      method: method,
      headers: { 'Content-Type': 'application/json' }
    };
    if (body) options.body = JSON.stringify(body);

    const response = await fetch(API_BASE + endpoint, options);
    if (!response.ok) throw new Error('API call failed');
    return await response.json();
  } catch (e) {
    console.error('API Error:', e);
    throw e;
  }
}

function getSession() {
  try {
    return JSON.parse(sessionStorage.getItem('crimson_user') || 'null');
  } catch {
    return null;
  }
}

function setSession(user) {
  sessionStorage.setItem('crimson_user', JSON.stringify(user));
}

function clearSession() {
  sessionStorage.removeItem('crimson_user');
}

function requireAuth(allowedRoles = []) {
  const user = getSession();
  if (!user) {
    window.location.href = '/login.html';
    return null;
  }
  if (allowedRoles.length && !allowedRoles.includes(user.role)) {
    window.location.href = '/index.html';
    return null;
  }
  return user;
}
