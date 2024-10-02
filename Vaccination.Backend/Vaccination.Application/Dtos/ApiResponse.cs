namespace Vaccination.Application.Dtos
{
    public record ApiResponse<T>(
       bool Success = true,
       T? Data = default,
       string? Message = null
   );
}