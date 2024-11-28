#include <stdio.h>

int main() {
    char input;
    printf("Input anything (Leave blank space to end): \n");
    while(1) {
        scanf(" %c", &input);
        if (input == ' ') {
            printf("Nothing in here. End. \n");
            break;
        } else if ((input >= 'a' && input <= 'z') || (input >= 'A' && input <= 'Z')) {
            printf("You just typed a letter.\n");
        } else if (input >= '0' && input <= '9') {
            printf("You just typed a number.\n");
        } else {
            printf("You just typed a special character.\n");
        }
    }

    return 0;
}