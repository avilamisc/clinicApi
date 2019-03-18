export class BaseService {
    protected getMultipartData(model: any): FormData {
        const formData = new FormData();

        for (const property in model) {
            if (model.hasOwnProperty(property)) {
                if (model[property] instanceof Array) {
                    formData.append(property, JSON.stringify(model[property]));
                } else {
                    if (model[property] != null) {
                        formData.append(property, model[property]);
                    }
                }
            }
        }

        return formData;
      }
}
