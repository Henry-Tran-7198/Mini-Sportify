#include <stdio.h>
int main(){
    float CDeg;
    float FDeg;
    printf("Conversion from C deg to F deg \n");
    printf("Input your C degree number: ");
    scanf("%f", &CDeg);
    FDeg = (CDeg * 1.8) + 32;
    printf("Successfully. %.2f ℃ has been converted into %.2f ℉ ", CDeg, FDeg);
}