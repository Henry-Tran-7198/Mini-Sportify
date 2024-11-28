#include <stdio.h>
int main() {
    printf("Numbers that divide with 9 from 200 to 300 is: \n");
    for (int i = 200; i <= 300; i++) {
        if (i % 9 == 0) {
            printf("%d ", i);
        }
    }
    printf("\n");
    return 0;
}