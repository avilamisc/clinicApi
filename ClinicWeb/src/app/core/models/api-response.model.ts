export class ApiResponse<T> {
    ErrorMessage: string;
    StatusCode: number;
    Data: T;
}
