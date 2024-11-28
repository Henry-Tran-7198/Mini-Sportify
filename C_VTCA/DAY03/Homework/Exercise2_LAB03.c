#include <stdio.h>
int main() {
int x;
int y;
printf("Input x: ");
scanf("%d", &x);
printf("x = %d \n", x);
printf("Input y: ");
scanf("%d", &y);
printf("y = %d \n", y);
int ex;
ex = x*x*x + 3*x*x + 3*x*y*y + y*y*y;
printf("Have: ex = x*x*x + 3*x*x + 3*x*y*y + y*y*y \n");
printf("Replace x = %d, y = %d => ex = %d", x, y, ex);
return 0;
}
