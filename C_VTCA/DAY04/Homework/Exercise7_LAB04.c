#include <stdio.h>
int main (){
    printf("       MENU     \n");
    printf("================== \n");
    printf("1. CF \n");
    printf("2. C \n");
    printf("3. HDJ \n");
    printf("4. DreamWeaver \n");
    printf("5. RDBMS \n");
    printf("6. Learn Java By Example \n");
    printf("================== \n");
    int choice;
    printf("Chọn: ");
    scanf("%d", &choice);
    if (choice < 1 || choice > 7){
        printf("Your choice is not on the list. \n");
    }
    else{
        switch(choice){
            case 1:
            printf("You chose CF!");
            break;
            case 2:
            printf("You chose C!");
            break;
            case 3:
            printf("You chose HDJ!");
            break;
            case 4:
            printf("You chose Dreamweaver!");
            break;
            case 5:
            printf("You chose RDBMS!");
            break;
            default:
            printf("You chose Learn Java By Example!");
            break;
        }
    }
}