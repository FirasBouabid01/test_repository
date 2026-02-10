export function getUserRole(): string | null {
    const token = localStorage.getItem("token");
    if (!token) return null;

    try {
        const parts = token.split(".");
        const payloadBase64 = parts[1];

        if (!payloadBase64) return null;

        const payload = JSON.parse(atob(payloadBase64));
        return payload.role ?? null;
    } catch (error) {
        console.error("Invalid JWT token", error);
        return null;
    }
}