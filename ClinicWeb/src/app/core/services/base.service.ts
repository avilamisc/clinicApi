import { CreateNotificationModel } from '../models';
import { NotificationService } from './notification/notification.service';

export class BaseService {
    constructor(private notificationBaseService: NotificationService = null) {
    }

    protected getMultipartData(model: any): FormData {
        const formData = new FormData();
        const d = new Date();
        d.toLocaleDateString();
        for (const property in model) {
            if (model.hasOwnProperty(property)) {
                if (model[property] instanceof Array) {
                    if (model[property] != null) {
                        formData.append(property, JSON.stringify(model[property]));
                    }
                } else if (model[property] instanceof Date) {
                    if (model[property] != null) {
                        console.log((model[property]).toLocaleDateString,
                        (model[property]).toUTCString,
                        (model[property]).toLocaleDateString)
                        formData.append(property, (model[property]).toISOString());
                    }
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
