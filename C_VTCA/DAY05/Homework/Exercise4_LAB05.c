#include <stdio.h>
int main(){
    printf("  PERSONAL FAVORITE  \n");
    printf("====================== \n");
    printf("1. Reading \n");
    printf("2. Listening to music \n");
    printf("3. Playing sports \n");
    printf("4. Computer \n");
    printf("5. End \n");
    printf("====================== \n");
    printf("Chọn: ");
    int input;
    while(1) {
        scanf("%d", &input);
        if (input == 5) {
            printf("SEE YOU AGAIN! \n");
            break;
        } else if (input == 1) {
            printf("YOU LIKE READING BOOKS! \n");
        } else if (input == 2) {
            printf("YOU LIKE LISTENING TO MUSIC! \n");
        } else if (input == 3) {
            printf("YOU LIKE PLAYING SPORTS! \n");
        } else {
            printf("YOU LIKE USING COMPUTER! \n");
        }
    }
    return 0;
}