import { CreateNotificationModel } from "../models";
import { NotificationService } from "./notification/notification.service";

export class BaseService {
    constructor(private notificationBaseService: NotificationService = null) {
    }
    
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

    protected withNotification(message: string, userId: number): void {
        const newNotification: CreateNotificationModel = {
            Content: message,
            UserId: userId
        };
        this.notificationBaseService.createNotification(newNotification)
            .subscribe();
    }
}
