# TravelPlanner

A modern, AI-powered travel planning web application that helps users design personalized itineraries, manage budgets, and explore destinations worldwide.

## Features

- **Trip Planning** — Multi-step wizard with destination selection, date/traveler configuration, budget preferences, and AI-generated itinerary simulation
- **Trip Management** — View, search, filter, and sort all planned trips with status tracking (Draft, Planning, Confirmed, Completed, Cancelled)
- **Trip Details** — Day-by-day itinerary, flight info, accommodation details, and budget breakdown in a tabbed interface
- **Budget & Expense Tracking** — Per-trip budget management with category breakdown, expense logging, and spending summaries
- **Destination Exploration** — Browse destinations with filters by continent, type (Cultural, Beach, Adventure, City Break, Nature), and budget level
- **User Authentication** — Login, registration with password strength validation, and profile management
- **Dark / Light Theme** — Toggle between themes with preference persistence
- **Responsive Design** — Fully responsive layout optimized for mobile, tablet, and desktop

## Tech Stack

- **HTML5** — Semantic markup
- **CSS3** — Custom properties, gradients, animations
- **JavaScript** — Vanilla JS with localStorage for state management
- **Bootstrap 5.3** — Responsive UI framework
- **Bootstrap Icons** — Icon library

## Project Structure

```
TravelPlanner/
├── index.html           # Homepage
├── login.html           # User login
├── register.html        # User registration
├── trips.html           # My Trips listing
├── new-trip.html        # Trip creation wizard
├── trip-details.html    # Individual trip view
├── explore.html         # Destination exploration
├── budget.html          # Budget & expense tracking
├── profile.html         # User profile & preferences
├── css/
│   └── style.css        # Global styles with CSS variables
├── js/
│   └── main.js          # Shared JavaScript utilities
└── docs/
    └── specificatii.html
```

## Getting Started

No build step or server required — just open `index.html` in your browser.

```bash
git clone git@github.com:bibozisbibogel/TravelPlanner.git
cd TravelPlanner
open index.html
```

## Screenshots

| Home | Explore | Trip Details |
|------|---------|--------------|
| ![Home](https://via.placeholder.com/300x200?text=Home) | ![Explore](https://via.placeholder.com/300x200?text=Explore) | ![Details](https://via.placeholder.com/300x200?text=Details) |

> Replace the placeholder images above with actual screenshots of the app.

## License

This project was developed as a university assignment (PAW - Web Application Programming).
