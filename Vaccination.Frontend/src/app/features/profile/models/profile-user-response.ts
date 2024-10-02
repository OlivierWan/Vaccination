export interface ProfileUserResponse {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  socialSecurityNumber: string;
  dateOfBirth: Date | null;
  city: string;
  nationality: string;
  address: string;
  postalCode: string;
  phoneNumber: string;
}

export interface ApiResponse {
  success: boolean;
  data: ProfileUserResponse;
  message: string;
}