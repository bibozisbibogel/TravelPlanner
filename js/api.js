/* =============================================
   TravelPlanner - API Service
   Centralized communication with the backend
   ============================================= */

const API_BASE = 'http://localhost:5168/api';

/**
 * Generic fetch wrapper with error handling.
 */
async function apiFetch(endpoint, options = {}) {
  const url = `${API_BASE}${endpoint}`;
  const config = {
    headers: { 'Content-Type': 'application/json' },
    ...options,
  };

  const response = await fetch(url, config);

  if (!response.ok) {
    const error = await response.json().catch(() => ({ message: response.statusText }));
    throw { status: response.status, ...error };
  }

  if (response.status === 204) return null;
  return response.json();
}

/* ===== Users ===== */
const UsersAPI = {
  register: (data) => apiFetch('/users/register', { method: 'POST', body: JSON.stringify(data) }),
  login: (email, password) => apiFetch('/users/login', { method: 'POST', body: JSON.stringify({ email, password }) }),
  getById: (id) => apiFetch(`/users/${id}`),
  update: (id, data) => apiFetch(`/users/${id}`, { method: 'PUT', body: JSON.stringify(data) }),
};

/* ===== Destinations ===== */
const DestinationsAPI = {
  getAll: () => apiFetch('/destinations'),
  getById: (id) => apiFetch(`/destinations/${id}`),
  create: (data) => apiFetch('/destinations', { method: 'POST', body: JSON.stringify(data) }),
  update: (id, data) => apiFetch(`/destinations/${id}`, { method: 'PUT', body: JSON.stringify(data) }),
  delete: (id) => apiFetch(`/destinations/${id}`, { method: 'DELETE' }),
};

/* ===== Trips ===== */
const TripsAPI = {
  getAll: (userId) => apiFetch(`/trips${userId ? '?userId=' + userId : ''}`),
  getById: (id) => apiFetch(`/trips/${id}`),
  create: (data) => apiFetch('/trips', { method: 'POST', body: JSON.stringify(data) }),
  update: (id, data) => apiFetch(`/trips/${id}`, { method: 'PUT', body: JSON.stringify(data) }),
  delete: (id) => apiFetch(`/trips/${id}`, { method: 'DELETE' }),
};

/* ===== Expenses ===== */
const ExpensesAPI = {
  getAll: (tripId) => apiFetch(`/expenses${tripId ? '?tripId=' + tripId : ''}`),
  getById: (id) => apiFetch(`/expenses/${id}`),
  create: (data) => apiFetch('/expenses', { method: 'POST', body: JSON.stringify(data) }),
  update: (id, data) => apiFetch(`/expenses/${id}`, { method: 'PUT', body: JSON.stringify(data) }),
  delete: (id) => apiFetch(`/expenses/${id}`, { method: 'DELETE' }),
};

/* ===== Accommodations ===== */
const AccommodationsAPI = {
  getAll: (tripId) => apiFetch(`/accommodations${tripId ? '?tripId=' + tripId : ''}`),
  getById: (id) => apiFetch(`/accommodations/${id}`),
  create: (data) => apiFetch('/accommodations', { method: 'POST', body: JSON.stringify(data) }),
  update: (id, data) => apiFetch(`/accommodations/${id}`, { method: 'PUT', body: JSON.stringify(data) }),
  delete: (id) => apiFetch(`/accommodations/${id}`, { method: 'DELETE' }),
};

/* ===== Itinerary Days ===== */
const ItineraryDaysAPI = {
  getAll: (tripId) => apiFetch(`/itinerarydays${tripId ? '?tripId=' + tripId : ''}`),
  getById: (id) => apiFetch(`/itinerarydays/${id}`),
  create: (data) => apiFetch('/itinerarydays', { method: 'POST', body: JSON.stringify(data) }),
  update: (id, data) => apiFetch(`/itinerarydays/${id}`, { method: 'PUT', body: JSON.stringify(data) }),
  delete: (id) => apiFetch(`/itinerarydays/${id}`, { method: 'DELETE' }),
};

/* ===== Itinerary Activities ===== */
const ItineraryActivitiesAPI = {
  getAll: (dayId) => apiFetch(`/itineraryactivities${dayId ? '?dayId=' + dayId : ''}`),
  getById: (id) => apiFetch(`/itineraryactivities/${id}`),
  create: (data) => apiFetch('/itineraryactivities', { method: 'POST', body: JSON.stringify(data) }),
  update: (id, data) => apiFetch(`/itineraryactivities/${id}`, { method: 'PUT', body: JSON.stringify(data) }),
  delete: (id) => apiFetch(`/itineraryactivities/${id}`, { method: 'DELETE' }),
};

/* ===== Weather (OpenWeatherMap via backend proxy) ===== */
const WeatherAPI = {
  getForecast: (city) => apiFetch(`/weather?city=${encodeURIComponent(city)}`),
};

/* ===== Search (SerpAPI via backend proxy) ===== */
const SearchAPI = {
  flights: (from, to, departDate, returnDate) =>
    apiFetch(`/search/flights?from=${encodeURIComponent(from)}&to=${encodeURIComponent(to)}&departDate=${departDate}${returnDate ? '&returnDate=' + returnDate : ''}`),
  hotels: (location, checkIn, checkOut) =>
    apiFetch(`/search/hotels?location=${encodeURIComponent(location)}&checkIn=${checkIn}&checkOut=${checkOut}`),
  restaurants: (location) =>
    apiFetch(`/search/restaurants?location=${encodeURIComponent(location)}`),
  places: (location) =>
    apiFetch(`/search/places?location=${encodeURIComponent(location)}`),
};
