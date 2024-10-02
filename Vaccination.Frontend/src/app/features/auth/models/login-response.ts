export interface LoginResponse {
  success: boolean;
  data: {
    expiration: string;
    isSucceed: boolean;
    message: string;
    refreshToken: string;
    token: string;
  };
  message: string;
}