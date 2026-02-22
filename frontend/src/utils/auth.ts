// src/utils/auth.ts

export function getToken(): string | null {
  return localStorage.getItem("token");
}

export function getUserRole(): string | null {
  const token = getToken();
  if (!token) return null;

  try {
    const parts = token.split(".");
    if (parts.length !== 3) return null;

    const payloadBase64 = parts[1];

    // ⚠️ إصلاح Base64Url
    const base64 = payloadBase64
      .replace(/-/g, "+")
      .replace(/_/g, "/");

    const payload = JSON.parse(atob(base64));

    // 🔥 .NET Role Claim
    return (
      payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] ||
      payload.role ||
      null
    );
  } catch (error) {
    console.error("Invalid JWT token", error);
    return null;
  }
}

export function isAuthenticated(): boolean {
  return !!getToken();
}