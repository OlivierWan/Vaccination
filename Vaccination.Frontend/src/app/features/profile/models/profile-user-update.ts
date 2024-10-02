export interface ProfileUserUpdateRequest {
  email: string;
  firstName: string;
  lastName: string;
  socialSecurityNumber: string;
  dateOfBirth: string | null;
  city: string;
  nationality: string;
  address: string;
  postalCode: string;
  phoneNumber: string;
}

export interface ApiResponse {
  success: boolean;
  message: string;
}
