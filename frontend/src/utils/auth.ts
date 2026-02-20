export function isAdmin(): boolean {
    const token = localStorage.getItem("token");
    if (!token) return false;

    try {
        const parts = token.split(".");
        const payloadBase64 = parts[1];

        if (!payloadBase64) return false;

        const payload = JSON.parse(atob(payloadBase64));
        return payload.is_admin === true || payload.is_admin === "true";
    } catch (error) {
        console.error("Invalid JWT token", error);
        return false;
    }
}