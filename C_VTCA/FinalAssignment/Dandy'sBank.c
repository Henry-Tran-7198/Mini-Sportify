#include <stdio.h>
#include <string.h>
#define MAX_ACCOUNTS 100
void UIMenu(){
    printf("=================================== \n");
    printf("Dandy's Bank \n");
    printf("=================================== \n");
}
void LogInSignUp(){
    printf("Sign up/Log in \n");
    printf("----------------------------------- \n");
    printf("Welcome to Dandy's Bank. \n");
    printf("Made by Dandy. \n");
    printf("This app is still in development. \n");
    printf("1. Sign up \n");
    printf("2. Log in \n");
    printf("3. Exit \n");
    printf("Alert: Sign up if you are first using this bank. \n");
    printf("       Log in if you have already had an account. \n");
}
typedef struct {
    char fullName[50];
    long long IDcard;
    char userName[50];
    int pinCard;
    double balance;
} Account;
void changePin(Account *account);
void printReceipt(Account account, const char *transactionType, double amount);
void getString(char *str, int length);
void printAccount(Account account);
int registerAccount(Account *accounts, int *count);
int login(Account *accounts, int count);
void deposit(Account *account);
void withdraw(Account *account);
void viewAccount(Account *account );
void saveAccountsToFile(Account *accounts, int count, const char *fileName);
int loadAccountsFromFile(Account *accounts, int *count, const char *fileName);
int main() {
    Account accounts[MAX_ACCOUNTS];
    int count = 0;
    int choice;
    loadAccountsFromFile(accounts, &count, "accounts.dat");
    do {
        UIMenu();
        LogInSignUp();
        printf("Your choice: ");
        scanf("%d", &choice);
        switch (choice) {
            case 1:
                printf("You chose sign up. Please wait. \n");
                UIMenu();
                registerAccount(accounts, &count);
                saveAccountsToFile(accounts, count, "accounts.dat");
                break;
            case 2:
                printf("You chose log in. Please wait. \n");
                UIMenu();
                printf("Log in \n");
                printf("----------------------------------- \n");
                if (login(accounts, count) == 1) {
                    printf("Successfully logged in!\n");
                }
                else{
                    printf("Invalid account. Please input again. \n");
                    while (login(accounts, count) != 1){
                        printf("Invalid account. Please input again. \n");
                    }
                    while (login(accounts, count) == 1){
                        printf("Successfully logged in! \n");
                    }
                }
                break;
            case 3:
                printf("Goodbye! See you again. \n");
                break;
            default:
                printf("Invalid choice. Please try again.\n");
        }
    } while (choice != 3);
    return 0;
}
void getString(char *str, int length) {
    getchar();
    fgets(str, length, stdin);
    str[strcspn(str, "\n")] = '\0';
}
int registerAccount(Account *accounts, int *count) {
    if (*count >= MAX_ACCOUNTS) {
        printf("Account limit reached.\n");
        return 0;
    }
    Account newAccount;
    int length = strlen(newAccount.userName);
    printf("Registering a new account: \n");
    printf("----------------------------------- \n");
    printf("Full name: ");
    getString(newAccount.fullName, 50);
    printf("ID Card: ");
    scanf("%lld", &newAccount.IDcard);
    while (newAccount.IDcard < 99999999999 || newAccount.IDcard > 999999999999){
        printf("Invalid ID card. Please input again. \n");
        printf("ID Card: ");
        scanf("%lld", &newAccount.IDcard);
    }
    printf("Username: ");
    getString(newAccount.userName, 50);
    while (length > 14){
        printf("Invalid username. Username usually has lower than 14 letters. Please input again. \n");
        printf("Username: ");
        getString(newAccount.userName, 50);
    }
    printf("Pin Card (6 digits): ");
    scanf("%d", &newAccount.pinCard);
    while (newAccount.pinCard < 99999 || newAccount.pinCard > 999999){
        printf("Invalid pin card. Pin card has 6 digits. Please input again. \n");
        printf("Pin Card (6 digits): ");
        scanf("%d", &newAccount.pinCard);
    }
    newAccount.balance = 0.0; 
    accounts[*count] = newAccount;
    (*count)++;
    printf("Account registered successfully! \n");
    printf("Heading you back to menu. Please wait... \n");
    return 1;
}
int login(Account *accounts, int count) {
    char userName[50];
    int pinCard;
    printf("Enter username: ");
    getString(userName, 50);
    printf("Enter pin card: ");
    scanf("%d", &pinCard);
    for (int i = 0; i < count; i++) {
        if (strcmp(accounts[i].userName, userName) == 0 && accounts[i].pinCard == pinCard) {
            printf("Successfully logged in. Please wait... \n");
            viewAccount(&accounts[i]);
            return 1;
        }
        
    }
    return 0;
}
void changePin(Account *account) {
    int currentPin, newPin;
    printf("Enter current pin card: ");
    scanf("%d", &currentPin);
    if (currentPin == account->pinCard) {
        do {
            printf("Enter new pin card (6 digits): ");
            scanf("%d", &newPin);
            if (newPin < 100000 || newPin > 999999) {
                printf("Invalid pin card. Pin card must be 6 digits.\n");
            }
        } while (newPin < 100000 || newPin > 999999);
    account->pinCard = newPin;
    printf("Pin card changed successfully!\n");
    } else {
        while (currentPin != account->pinCard){
            printf("Current pin card is incorrect.\n");
            int currentPin, newPin;
            printf("Enter current pin card: ");
            scanf("%d", &currentPin);
            if (currentPin == account->pinCard) {
                do {
                printf("Enter new pin card (6 digits): ");
                scanf("%d", &newPin);
                if (newPin < 100000 || newPin > 999999) {
                    printf("Invalid pin card. Pin card must be 6 digits.\n");
                }
                } while (newPin < 100000 || newPin > 999999);
                    account->pinCard = newPin;
                    printf("Pin card changed successfully!\n");
                    break;
            }
        }
    }
}
void deposit(Account *account) {
    double amount;
    printf("Enter amount to deposit: ");
    scanf("%lf", &amount);
    if (amount > 0) {
        account->balance += amount;
        printf("Deposited %.2f. New balance: %.2f\n", amount, account->balance);
    } else {
        printf("Invalid amount.\n");
    }
}
void withdraw(Account *account) {
    int choice;
    double amount;
    printf("1. 100.000 \n");
    printf("2. 200.000 \n");
    printf("3. 500.000 \n");
    printf("4. 1.000.000 \n");
    printf("5. 2.000.000 \n");
    printf("6. Other number \n");
    printf("Your choice to withdraw: ");
    scanf("%d", &choice);
    switch(choice){
        case 1:
            amount = 100000;
                if (amount > 0 && amount <= account->balance) {
                    account->balance -= amount;
                    printf("Withdrew %.2f. New balance: %.2f\n", amount, account->balance);
                    printf("Do you want to print? \n");
                    printf("1. Yes \n");
                    printf("2. No \n");
                    int choiceee;
                    printf("Choice: ");
                    scanf("%d", &choiceee);
                    if (choiceee == 1){
                        printReceipt(*account, "Withdraw", amount);
                    }
                    else if(choiceee == 2){
                        printf("Thank you for using our bank! \n");
                    }
                    else {
                        while (choiceee != 1 && choiceee != 2){
                            printf("Invalid choice. Please input again. \n");
                            printf("Do you want to print? \n");
                            printf("1. Yes \n");
                            printf("2. No \n");
                            int choiceee;
                            printf("Choice: ");
                            scanf("%d", &choiceee);
                            if (choiceee == 1){
                                printReceipt(*account, "Withdraw", amount);
                            }
                            else if(choiceee == 2){
                                printf("Thank you for using our bank! \n");
                            }
                        } 
                    }
                }else {
                    printf("Invalid amount or insufficient balance.\n");
                }
        break;
        case 2:
            amount = 200000;
            if (amount > 0 && amount <= account->balance) {
                account->balance -= amount;
                printf("Withdrew %.2f. New balance: %.2f\n", amount, account->balance);
                printf("Do you want to print? \n");
                printf("1. Yes \n");
                printf("2. No \n");
                int choiceee;
                printf("Choice: ");
                scanf("%d", &choiceee);
                if (choiceee == 1){
                    printReceipt(*account, "withdraw", amount);
                }
                else if(choiceee == 2){
                    printf("Thank you for using our bank! \n");
                }
                else{
                    printf("Invalid choice. Please input again. \n");
                }
            } else {
                printf("Invalid amount or insufficient balance.\n");
            }
        break;
        case 3:
            amount = 500000;
            if (amount > 0 && amount <= account->balance) {
                account->balance -= amount;
                printf("Withdrew %.2f. New balance: %.2f\n", amount, account->balance);
                printf("Do you want to print? \n");
                printf("1. Yes \n");
                printf("2. No \n");
                int choiceee;
                printf("Choice: ");
                scanf("%d", &choiceee);
                if (choiceee == 1){
                    printReceipt(*account, "withdraw", amount);
                }
                else if(choiceee == 2){
                    printf("Thank you for using our bank! \n");
                }
                else{
                    printf("Invalid choice. Please input again. \n");
                }
            } else {
                printf("Invalid amount or insufficient balance.\n");
            }
        break;
        case 4:
            amount = 1000000;
            if (amount > 0 && amount <= account->balance) {
                account->balance -= amount;
                printf("Withdrew %.2f. New balance: %.2f\n", amount, account->balance);
                printf("Do you want to print? \n");
                printf("1. Yes \n");
                printf("2. No \n");
                int choiceee;
                printf("Choice: ");
                scanf("%d", &choiceee);
                if (choiceee == 1){
                    printReceipt(*account, "withdraw", amount);
                }
                else if(choiceee == 2){
                    printf("Thank you for using our bank! \n");
                }
                else{
                    printf("Invalid choice. Please input again. \n");
                }
            } else {
                printf("Invalid amount or insufficient balance.\n");
            }
        break;
        case 5:
            amount = 2000000;
            if (amount > 0 && amount <= account->balance) {
                account->balance -= amount;
                printf("Withdrew %.2f. New balance: %.2f\n", amount, account->balance);
                printf("Do you want to print? \n");
                printf("1. Yes \n");
                printf("2. No \n");
                int choiceee;
                printf("Choice: ");
                scanf("%d", &choiceee);
                if (choiceee == 1){
                    printReceipt(*account, "withdraw", amount);
                }
                else if(choiceee == 2){
                    printf("Thank you for using our bank! \n");
                }
                else{
                    printf("Invalid choice. Please input again. \n");
                }
            } else {
                printf("Invalid amount or insufficient balance.\n");
            }
        break;
        case 6:
            printf("Amount money you want to withdraw: ");
            scanf("%lf", &amount);
            if (amount > 0 && amount <= account->balance) {
                account->balance -= amount;
                printf("Withdrew %.2f. New balance: %.2f\n", amount, account->balance);
                printf("Do you want to print? \n");
                printf("1. Yes \n");
                printf("2. No \n");
                int choiceee;
                printf("Choice: ");
                scanf("%d", &choiceee);
                if (choiceee == 1){
                    printReceipt(*account, "withdraw", amount);
                }
                else if(choiceee == 2){
                    printf("Thank you for using our bank! \n");
                }
                else{
                    printf("Invalid choice. Please input again. \n");
                }
            } else {
                printf("Invalid amount or insufficient balance.\n");
        break;
            }
    }
}
void transfer(Account *account){
    long long accounttotransfer;
    double amount;
    int choice;
    printf("Transfer to account: ");
    scanf("%lld", &accounttotransfer);
    printf("Money to transfer: ");
    scanf("%lf", &amount);
    if (amount > 0 && amount <= account->balance) {
        account->balance -= amount;
        printf("Successfully. You has transfered %.2f to account %lld. \n", amount, accounttotransfer);
        printf("Your new balance: %.2f \n", account->balance);
    } else {
        printf("Invalid amount or insufficient balance.\n");
    }
}
void printReceipt(Account account, const char *transactionType, double amount) {
    UIMenu();
    printf("      Receipt Withdraw at ATM \n");
    printf("----------------------------------- \n");
    printf("Date: 28/05/2077        Time: 00:00 \n");
    printf("Transaction Type: %s\n", transactionType);
    printf("Account ID:             %lld \n", account.IDcard);
    printf("Withdraw ID:         01234678910112 \n");
    printf("Amount:                 %.2f\n", amount);
    printf("Content: Withdraw money at ATM. \n");
    printf("----------------------------------- \n");
    printf("New Balance:        %.2f\n", account.balance);
    printf("----------------------------------- \n");
    printf("     Thanks for using our bank!    \n");
}
void viewAccount(Account *account) {
    UIMenu();
    printf("ATM Machine \n");
    printf("----------------------------------- \n");
    printf("Account no: %lld\n", account->IDcard);
    printf("Pin card: ****** \n");
    printf("----------------------------------- \n");
    printf("Full Name: %s\n", account->fullName);
    printf("----------------------------------- \n");
    int choice;
    do {
        UIMenu();
        printf("ATM Machine \n");
        printf("----------------------------------- \n");
        printf("6: %lld\n", account->IDcard);
        printf("Pin card: ****** \n");
        printf("----------------------------------- \n");
        printf("Full Name: %s\n", account->fullName);
        printf("----------------------------------- \n");
        printf("1. Deposit\n");
        printf("2. Withdraw\n");
        printf("3. Transfer\n");
        printf("4. Change PIN \n");
        printf("5. End of transaction \n");
        printf("Choose an option: ");
        scanf("%d", &choice);
        switch (choice) {
            case 1:
                deposit(account);
                break;
            case 2:
                withdraw(account);
                break;
            case 3:
                transfer(account);
                break;
            case 4:
                changePin(account);
                break;
            case 5:
                printf("Logging out...\n");
                break;
            default:
                printf("Invalid choice. Please try again.\n");
        }
    } while (choice != 5);
}
void saveAccountsToFile(Account *accounts, int count, const char *fileName) {
    FILE *file = fopen(fileName, "wb");
    if (file) {
        fwrite(&count, sizeof(int), 1, file);
        fwrite(accounts, sizeof(Account), count, file);
        fclose(file);
    } else {
        printf("Error opening file for writing.\n");
    }
}
int loadAccountsFromFile(Account *accounts, int *count, const char *fileName) {
    FILE *file = fopen(fileName, "rb");
    if (file) {
        fread(count, sizeof(int), 1, file);
        fread(accounts, sizeof(Account), *count, file);
        fclose(file);
        return 1;
    }
    return 0;
}