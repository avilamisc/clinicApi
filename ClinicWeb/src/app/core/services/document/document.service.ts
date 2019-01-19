import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { saveAs } from 'file-saver';

import { ApiRoutes } from 'src/app/utilities/api-routes';

@Injectable({
  providedIn: 'root'
})
export class DocumentService {

  constructor(private http: HttpClient) { }

    public downloadDocument(id: number): Observable<Blob> {
      return this.http.get<Blob>(`${ApiRoutes.documents}/${id}`, { responseType: 'blob' as 'json' })
        .pipe(map(result => {
            saveAs(result, 'download', { type: result.type });
            return result;
        }));
    }
}
