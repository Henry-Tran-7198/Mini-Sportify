#include <stdio.h>
#include <stdlib.h>
typedef struct Node{
    int data;
    struct Node *next;
} Node;
Node* createNode(int value){
    Node* newNode = (Node*)malloc(sizeof(Node));
    newNode->data = value;
    newNode->next = NULL;
    return newNode;
}
void printList(Node *head){
    Node* temp = head;
    while (temp != NULL){
        printf("%d -> ", temp->data);
        temp = temp->next;
    }
    printf("NULL \n");
}
void deleteHead(Node **head){
    if (*head == NULL) return;
    Node *temp = *head;
    *head = (*head)->next;
    free(temp);
}
int main(int argc, char const *argv[])
{   
    printf("Bài 1: \n");
    Node* head = createNode(10);
    head->next = createNode(20);
    head->next->next = createNode(30);
    printList(head);
    printf("Bài 4: \n");
    printf("List before deleting: \n");
    printList(head);
    printf("List after deleting: \n");
    deleteHead(&head);
    printList(head);
    return 0;
}
