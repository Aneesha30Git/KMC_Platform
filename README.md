# KMC Municipal Council Event Platform

A full-stack dual-portal web application built for a municipal council, featuring a public event portal for citizens and a separate organizer portal for council staff.

## 🌐 Overview
This platform consists of two websites working together through a shared REST API backend:
- **Public Portal** — Citizens can browse, view, and register for upcoming municipal events
- **Organizer Portal** — Council staff can create, manage, track, and update events

## 🛠️ Tech Stack
- **Frontend:** HTML, CSS, JavaScript
- **Backend:** C# .NET Web API
- **Authentication:** JWT (JSON Web Tokens)
- **Architecture:** RESTful Service-Oriented Architecture (SOA)

## ✨ Features
- User registration and login with JWT authentication
- Role-based access control (public user vs organizer)
- Event creation, editing, and deletion
- Event registration for citizens
- Organizer dashboard to track registrations
- Responsive design for desktop and mobile

## 🚀 How to Run
1. Clone the repository
2. Open the solution in Visual Studio
3. Update `appsettings.json` with your database connection string
4. Run `Update-Database` in Package Manager Console
5. Press F5 to run the API
6. Open the frontend HTML files in your browser

## 📁 Project Structure
KMC_API/
├── Controllers/
├── Models/
├── DTOs/
├── Services/
├── Data/
└── wwwroot/

## 👩‍💻 Developer
Developed by Aneesha — ICBT Campus, Cardiff Metropolitan University
