export class ApiResponse<T> {
    RrrorMessage: string;
    StatusCode: number;
    Result: T;
}
