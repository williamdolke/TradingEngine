# Overview 

A Trading Engine server that simulates an exchange. You can create instruments, and each of them will have order books that you can execute trades on. Currently using a FIFO price time priority matching algorithm.

# Features
The following features are currently supported.

### Order Types

1. New Order
2. Modify Order
3. Cancel Order

### Matching Algorithms

1. First-In-First-Out (FIFO)

# Setup/Installation

1. Clone the TradingEngine repository:
```sh
git clone https://github.com/williamdolke/TradingEngine
```

2. Change to the project directory:
```sh
cd TradingEngine
```

3. Install the dependencies:
```sh
dotnet build
```

## Running TradingEngine

```sh
dotnet run
```

## Interacting with the orderbook
The general command format to interact with the orderbook is:
```sh
orderType username securityID OrderID price quantity side
```

### Buy
```sh
new user1 999 1 99 10 buy
```

### Sell
```sh
new user1 999 2 101 10 sell
```

### Cancel
```sh
cancel user1 999 1
```

### Modify
```sh
modify user1 999 2 98 11 sell
```

## Tests
```sh
dotnet test
```

## Contributing

Contributions are always welcome! Please follow these steps:
1. Fork the project repository. This creates a copy of the project on your account that you can modify without affecting the original project.
2. Clone the forked repository to your local machine using a Git client like Git or GitHub Desktop.
3. Create a new branch with a descriptive name (e.g., `new-feature-branch` or `bugfix-issue-123`).
```sh
git checkout -b new-feature-branch
```
4. Make changes to the project's codebase.
5. Commit your changes to your local branch with a clear commit message that explains the changes you've made.
```sh
git commit -m 'Implemented new feature.'
```
6. Push your changes to your forked repository on GitHub using the following command
```sh
git push origin new-feature-branch
```
7. Create a new pull request to the original project repository. In the pull request, describe the changes you've made and why they're necessary.
The project maintainers will review your changes and provide feedback or merge them into the main branch.
