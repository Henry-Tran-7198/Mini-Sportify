#include <stdio.h>
int main(){
    float length;
    float width;
    printf("Input length: ");
    scanf("%f", &length);
    printf("Input width: ");
    scanf("%f", &width);
    float perimeter = 0;
    perimeter = (length + width)*2;
    float area = 1;
    area = length*width;
    printf("Perimeter of the rectangle is: %.2f \n", perimeter);
    printf("Area of the rectangle is: %.2f \n", area);
}