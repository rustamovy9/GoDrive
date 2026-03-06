```md
# 🚗 GoDrive — AI Powered Car Rental Platform

GoDrive — backend система платформы аренды автомобилей, разработанная на .NET 8.
Система позволяет пользователям арендовать автомобили, владельцам добавлять машины,
администраторам проверять документы, а AI-ассистент помогает подобрать автомобиль.

--------------------------------------------------

FEATURES

Users
- registration and login
- JWT authentication
- profile management
- avatar upload

Cars
- add cars
- update cars
- upload car images
- upload car documents
- manage car availability

Document Verification
- admin verifies documents
- approve or reject documents
- automatic car status update

Car Status

PendingApproval
Available
Rented
Maintenance
Blocked

--------------------------------------------------

AI ASSISTANT

AI assistant helps users:
- choose cars
- compare vehicles
- recommend cars by budget
- answer questions about cars

Example

User:
I need a cheap sedan in Dushanbe

AI:
Here are cars you may like
- Toyota Camry
- Hyundai Sonata
- Kia K5

--------------------------------------------------

ARCHITECTURE

Clean Architecture

src

Domain
  Entities
  Enums
  Common

Application
  DTOs
  Contracts
  Services
  Mappers

Infrastructure
  DataAccess
  Repositories
  ExternalServices
  AIIntegration

API
  Controllers
  Middleware
  Configuration

--------------------------------------------------

TECH STACK

.NET 8
ASP.NET Core
Entity Framework Core
PostgreSQL (Neon)
Supabase Storage
Render
JWT Authentication
Google Gemini AI

--------------------------------------------------

STORAGE

Files stored in Supabase

Car Images
Car Documents
User Avatars

Database stores only file name

API returns full URL

Example

https://xxxxx.supabase.co/storage/v1/object/public/images/file.jpg

--------------------------------------------------

PRICING SYSTEM

Each car has price history

Latest price is returned using

CarPrices
.OrderByDescending(x => x.CreatedAt)
.Select(x => x.PricePerDay)
.FirstOrDefault()

--------------------------------------------------

AUTHENTICATION

JWT

Authorization: Bearer {token}

--------------------------------------------------

DEPLOYMENT

Backend
Render

Database
Neon PostgreSQL

Storage
Supabase Storage

--------------------------------------------------

API RESPONSE EXAMPLE

{
  "id": 7,
  "brand": "Mercedes",
  "model": "C63 AMG",
  "year": 2016,
  "status": "Available",
  "location": "Tajikistan, Dushanbe",
  "currentPricePerDay": 1500,
  "images": [
    "https://supabase/images/car1.jpg"
  ]
}

--------------------------------------------------

FUTURE IMPROVEMENTS

booking system
payments
notifications
AI car comparison
AI price prediction
mobile app

--------------------------------------------------

AUTHOR

Akai Yusuf
Backend Developer (.NET)

GitHub
https://github.com/rustamovy9
```
