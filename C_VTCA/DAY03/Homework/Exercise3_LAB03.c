#include <stdio.h>
#include<math.h> 
int main() 
{
    int i;
    int j; 
    i = 5;
    j = 7;
    printf("i = %d \n", i);
    printf("j = %d \n", j);
    float num = (float) i / j;
    printf("%0.7f \n", num);
    return 0;
}