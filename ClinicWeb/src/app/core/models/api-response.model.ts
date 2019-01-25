export class ApiResponse<T> {
    RrrorMessage: string;
    StatusCode: number;
    Data: T;
}
