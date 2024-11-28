#include <stdio.h>
#include <math.h>
int main(){
    const double PI = 3.141592653589793;
    int R;
    int h;
    printf("Input your R: ");
    scanf("%d", &R);
    printf("Input your h: ");
    scanf("%d", &h);
    int S;
    S = PI * R * R;
    printf("Base area of a circular cylinder: %d \n", S);
    int V;
    V = PI * R * R * h;
    printf("Volume of a circular cylinder: %d", V);
}