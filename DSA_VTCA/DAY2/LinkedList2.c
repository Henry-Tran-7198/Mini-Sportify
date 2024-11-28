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
void insertAtHead(Node** head, int value){
    Node* newNode = createNode(value);
    newNode->next = *head;
    *head = newNode;
}
void insertAtEnd(Node **head, int value){
    Node* newNode = createNode(value);
    if (*head == NULL){\
        *head = newNode;
    }
    Node* temp = *head;
    while (temp->next != NULL){
        temp = temp->next;
    }
    temp->next = newNode;
}

int main(int argc, char const *argv[])
{   
    printf("Bài 2: \n");
    Node* head = NULL;
    insertAtHead(&head, 10);
    insertAtHead(&head, 50);
    insertAtHead(&head, 30);
    printList(head);
    printf("Bài 3: \n");
    insertAtEnd(&head, 190);
    insertAtEnd(&head, 360);
    insertAtEnd(&head, 305);
    printList(head);
    return 0;
}