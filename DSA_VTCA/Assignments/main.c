#include <stdio.h>
#include <stdlib.h>
#include <string.h>

typedef struct MobilePhone {
    int mobile_id;
    char name[50];
    double price;
    struct MobilePhone* next;
} MobilePhone;
MobilePhone* head = NULL;
void addMobilePhone(int id, const char* name, double price) {
    MobilePhone* newPhone = (MobilePhone*)malloc(sizeof(MobilePhone));
    newPhone->mobile_id = id;
    strcpy(newPhone->name, name);
    newPhone->price = price;
    newPhone->next = head;
    head = newPhone;
    printf("Mobile phone added successfully!\n");
}
MobilePhone* searchMobile(int id) {
    MobilePhone* current = head;
    while (current != NULL) {
        if (current->mobile_id == id) {
            return current;
        }
        current = current->next;
    }
    return NULL;
}

void updateMobile(int id, const char* name, double price) {
    MobilePhone* phone = searchMobile(id);
    if (phone) {
        strcpy(phone->name, name);
        phone->price = price;
        printf("Mobile phone updated successfully!\n");
    } else {
        printf("Mobile phone not found!\n");
    }
}

void deleteMobile(int id) {
    MobilePhone* current = head;
    MobilePhone* previous = NULL;
    while (current != NULL && current->mobile_id != id) {
        previous = current;
        current = current->next;
    }
    if (current == NULL) {
        printf("Mobile phone not found!\n");
        return;
    }
    if (previous == NULL) {
        head = current->next;
    } else {
        previous->next = current->next;
    }
    free(current);
    printf("Mobile phone deleted successfully!\n");
}
void displayAllMobilePhones() {
    MobilePhone* current = head;
    if (current == NULL) {
        printf("No mobile phones available.\n");
        return;
    }
    printf("Mobile Phones:\n");
    while (current != NULL) {
        printf("ID: %d, Name: %s, Price: %.2f\n", current->mobile_id, current->name, current->price);
        current = current->next;
    }
}

void displayTop5HighestPrice() {
    if (head == NULL) {
        printf("No mobile phones available.\n");
        return;
    }
    MobilePhone* current = head;
    MobilePhone* phones[100];
    int count = 0;

    while (current != NULL) {
        phones[count++] = current;
        current = current->next;
    }
    for (int i = 0; i < count - 1; i++) {
        for (int j = i + 1; j < count; j++) {
            if (phones[i]->price < phones[j]->price) {
                MobilePhone* temp = phones[i];
                phones[i] = phones[j];
                phones[j] = temp;
            }
        }
    }
    printf("Top 5 highest price mobile phones:\n");
    for (int i = 0; i < count && i < 5; i++) {
        printf("ID: %d, Name: %s, Price: %.2f\n", phones[i]->mobile_id, phones[i]->name, phones[i]->price);
    }
}
void displayMenu() {
    printf("----------- Mobile Phone Shop ---------\n");
    printf("=======================================\n");
    printf("1. Add mobile phone\n");
    printf("2. Search mobile phone\n");
    printf("3. Update mobile\n");
    printf("4. Delete mobile phone\n");
    printf("5. Shop reports\n");
    printf("0. Exit\n");
    printf("#Choose: ");
}
int main() {
    int choice;
    while (1) {
        displayMenu();
        scanf("%d", &choice);

        if (choice == 0) break;

        if (choice == 1) {
            int id;
            char name[50];
            double price;
            printf("Enter mobile ID: ");
            scanf("%d", &id);
            printf("Enter mobile name: ");
            scanf("%s", name);
            printf("Enter mobile price: ");
            scanf("%lf", &price);
            addMobilePhone(id, name, price);
        } else if (choice == 2) {
            int id;
            printf("Enter mobile ID to search: ");
            scanf("%d", &id);
            MobilePhone* phone = searchMobile(id);
            if (phone) {
                printf("Found: ID: %d, Name: %s, Price: %.2f\n", phone->mobile_id, phone->name, phone->price);
            } else {
                printf("Mobile phone not found!\n");
            }
        } else if (choice == 3) {
            int id;
            char name[50];
            double price;
            printf("Enter mobile ID to update: ");
            scanf("%d", &id);
            printf("Enter new mobile name: ");
            scanf("%s", name);
            printf("Enter new mobile price: ");
            scanf("%lf", &price);
            updateMobile(id, name, price);
        } else if (choice == 4) {
            int id;
            printf("Enter mobile ID to delete: ");
            scanf("%d", &id);
            deleteMobile(id);
        } else if (choice == 5) {
            int reportChoice;
            printf("1. Display all mobile phones\n");
            printf("2. Display top 5 highest price mobile phones\n");
            printf("0. Back to main menu\n");
            printf("#Choose: ");
            scanf("%d", &reportChoice);
            if (reportChoice == 1) {
                displayAllMobilePhones();
            } else if (reportChoice == 2) {
                displayTop5HighestPrice();
            }
        } else {
            printf("Invalid choice! Please try again.\n");
        }
    }
    while (head) {
        MobilePhone* temp = head;
        head = head->next;
        free(temp);
    }

    return 0;
}