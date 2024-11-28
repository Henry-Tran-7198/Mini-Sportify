#include <stdio.h>
#include <stdlib.h>
int main() {
    int n;
    int i;
    printf("Nhập số phần tử của mảng: ");
    scanf("%d", &n);
    int arr = (int)malloc(n * sizeof(int));
    if (arr == NULL) {
        printf("Không đủ bộ nhớ!");
        return 1;
    }
    for (int i = 0; i < n; i++) {
        printf("Nhap phan tu thu %d: ", i + 1);
        scanf("%d", &arr[i]);
    }
    printf("Mảng: ");
    for (int i = 0; i < n; i++) {
        printf("%d ", arr[i]);
    }
}