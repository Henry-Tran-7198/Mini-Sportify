#include<stdio.h>  
#include<conio.h>
int main(){  
int a = 0, b = 1, c, i, n;  
printf("Please input the numbers of elements in Fibonacci numbers list: ");  
scanf("%d", &n);  
printf("\n%d %d", a, b);
for(i = 2; i < n; ++i){  
  c = a + b;  
  printf(" %d",c);  
  a = b;
  b = c;
}
return 0;
}