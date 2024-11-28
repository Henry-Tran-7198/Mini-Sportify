#include <stdio.h>
int main(){
    int xA;
    int yA;
    int xB;
    int yB;
    int xC;
    int yC;
    printf("Input xA = ");
    scanf("%d", xA);
    printf("Input yA = ");
    scanf("%d", yA);
    printf("Input xB = ");
    scanf("%d", xB);
    printf("Input yB = ");
    scanf("%d", yB);
    printf("Input xC = ");
    scanf("%d", xC);
    printf("Input yC = ");
    scanf("%d", yC);
    printf("=> A(%d, %d)", xA, yA);
    printf("=> B(%d, %d)", xB, yB);
    printf("=> C(%d, %d)", xC, yC);
    int xAB = xB - xA;
    int yAB = yB - yA;
    int xAC = xC - xA;
    int yAC = yC - yA;
    if (xAB*yAC == xAC*yAB){
        print("3 points A, B, C are collinear. \n");
    }
    else{
        print("3 points A, B, C are not collinear. ");
    }
}