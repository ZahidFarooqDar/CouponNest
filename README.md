# Coupon Next

## Overview
**Coupon Next** is a dynamic platform that allows users to **buy and sell unused coupons** securely and efficiently. It provides a seamless experience for individuals looking to monetize their unused discounts while offering buyers a chance to save on their favorite brands.

## Features
- 🔹 **User Authentication & Authorization**
  - Secure login & registration
  - Role-based access control (buyers & sellers)

- 🔹 **Coupon Listing & Management**
  - Users can list unused coupons with details such as discount value, expiration date, and seller price
  - Buyers can browse, search, and filter available coupons

- 🔹 **Secure Transactions**
  - Integrated **Stripe API** for seamless payments
  - Secure checkout and transaction history tracking

- 🔹 **Coupon Verification System**
  - Ensures validity of listed coupons before transactions
  - Buyers can report invalid or expired coupons

- 🔹 **Admin Dashboard**
  - Manage users, coupons, and transactions
  - View analytics and reports

## Tech Stack
- **Backend:** ASP.NET Core Web API, C#
- **Database:** SQL Server / MySQL
- **Payment Integration:** Stripe API

## Installation & Setup
### Prerequisites
- .NET SDK
- SQL Server
- Stripe API keys


### Steps
1. **Clone the repository:**
   ```sh
   git clone https://github.com/ZahidFarooqDar/CouponNext.git
   cd CouponNext
   ```
2. **Set up database:**
   - Configure connection string in `appsettings.json`
   

3. **Run the application:**
   ```sh
   dotnet run
   ```

## API Endpoints (Sample)
| Method | Endpoint | Description |
|--------|----------|------------|
| GET | `/api/coupons` | Fetch all coupons |
| POST | `/api/coupons` | Add a new coupon |
| GET | `/api/coupons/{id}` | Fetch coupon details |
| DELETE | `/api/coupons/{id}` | Delete a coupon |
| POST | `/api/transactions` | Purchase a coupon |

## Contribution
Feel free to contribute by forking the repository and submitting a pull request. Ensure code follows best practices and includes proper documentation.

## License
MIT License

## Contact
📧 **Email:** [raahizaahid@gmail.com](mailto:raahizaahid@gmail.com)
📌 **LinkedIn:** [Zahid Farooq Dar](https://www.linkedin.com/in/zahid-farooq-dar/)
