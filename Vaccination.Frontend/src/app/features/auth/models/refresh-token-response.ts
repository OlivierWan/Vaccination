export interface RefreshTokenResponse {
    success: boolean;
    data: {
        expiration: string;
        refreshToken: string;
        token: string;
    };
    message: string;
    }