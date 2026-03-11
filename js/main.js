/* =============================================
   TravelPlanner - Shared JavaScript
   ============================================= */

// Default mock user (simulates a logged-in session)
const DEFAULT_USER = {
  name: 'Cristian Stoian',
  email: 'cristian.stoian@gmail.com',
  avatar: 'CS'
};

/**
 * Reads the current user from localStorage.
 * Falls back to the default mock user so all pages show a logged-in state.
 */
function getCurrentUser() {
  try {
    const stored = localStorage.getItem('tp_user');
    return stored ? JSON.parse(stored) : DEFAULT_USER;
  } catch {
    return DEFAULT_USER;
  }
}

/** Saves a user object to localStorage */
function setCurrentUser(user) {
  localStorage.setItem('tp_user', JSON.stringify(user));
}

/** Clears the session and redirects to login */
function logout() {
  localStorage.removeItem('tp_user');
  window.location.href = 'login.html';
}

/**
 * Renders the navbar user badge (name + email + logout link).
 * Expects an element with id="navUser" in every page.
 */
function renderNavUser() {
  const container = document.getElementById('navUser');
  if (!container) return;

  const user = getCurrentUser();
  const initials = user.name
    .split(' ')
    .map(p => p[0])
    .join('')
    .toUpperCase()
    .slice(0, 2);

  container.innerHTML = `
    <li class="nav-item dropdown d-flex align-items-center">
      <a class="nav-link dropdown-toggle d-flex align-items-center gap-2 user-badge"
         href="#" id="userDropdown" role="button"
         data-bs-toggle="dropdown" aria-expanded="false">
        <span class="rounded-circle bg-success text-white d-inline-flex align-items-center justify-content-center fw-bold"
              style="width:32px;height:32px;font-size:0.8rem;">${initials}</span>
        <span>
          <span class="d-block user-name">${user.name}</span>
          <span class="d-block user-email">${user.email}</span>
        </span>
      </a>
      <ul class="dropdown-menu dropdown-menu-end shadow" aria-labelledby="userDropdown">
        <li>
          <a class="dropdown-item" href="profile.html">
            <i class="bi bi-person-circle me-2"></i>Profile
          </a>
        </li>
        <li>
          <a class="dropdown-item" href="trips.html">
            <i class="bi bi-suitcase-lg me-2"></i>My Trips
          </a>
        </li>
        <li><hr class="dropdown-divider"></li>
        <li>
          <a class="dropdown-item text-danger" href="#" onclick="logout(); return false;">
            <i class="bi bi-box-arrow-right me-2"></i>Log out
          </a>
        </li>
      </ul>
    </li>
  `;
}

/**
 * Highlights the active nav link based on the current page filename.
 */
function setActiveNavLink() {
  const currentPage = window.location.pathname.split('/').pop() || 'index.html';
  document.querySelectorAll('.navbar-nav .nav-link').forEach(link => {
    const href = link.getAttribute('href');
    if (href === currentPage) {
      link.classList.add('active');
      link.setAttribute('aria-current', 'page');
    }
  });
}

/* =============================================
   Dark / Light Theme Toggle
   ============================================= */

/** Applies the saved theme (or default light) on page load */
function applyTheme() {
  const saved = localStorage.getItem('tp_theme') || 'light';
  document.documentElement.setAttribute('data-theme', saved);
  updateThemeIcon(saved);
}

/** Toggles between light and dark theme */
function toggleTheme() {
  const current = document.documentElement.getAttribute('data-theme') || 'light';
  const next = current === 'dark' ? 'light' : 'dark';
  document.documentElement.setAttribute('data-theme', next);
  localStorage.setItem('tp_theme', next);
  updateThemeIcon(next);
}

/** Updates the toggle button icon */
function updateThemeIcon(theme) {
  const btn = document.getElementById('themeToggle');
  if (!btn) return;
  btn.innerHTML = theme === 'dark'
    ? '<i class="bi bi-sun-fill"></i>'
    : '<i class="bi bi-moon-fill"></i>';
  btn.title = theme === 'dark' ? 'Switch to light mode' : 'Switch to dark mode';
}

/**
 * Renders the theme toggle button in the navbar.
 * Expects an element with id="themeToggleWrap" in every page.
 */
function renderThemeToggle() {
  const wrap = document.getElementById('themeToggleWrap');
  if (!wrap) return;
  wrap.innerHTML = `
    <button class="theme-toggle" id="themeToggle" onclick="toggleTheme()" title="Toggle theme">
      <i class="bi bi-moon-fill"></i>
    </button>
  `;
  // Update icon to match current theme
  const current = document.documentElement.getAttribute('data-theme') || 'light';
  updateThemeIcon(current);
}

/** Initialize on every page */
document.addEventListener('DOMContentLoaded', () => {
  applyTheme();
  // Ensure a mock user exists in localStorage so all pages appear "logged in"
  if (!localStorage.getItem('tp_user')) {
    setCurrentUser(DEFAULT_USER);
  }
  renderThemeToggle();
  renderNavUser();
  setActiveNavLink();
});

/* =============================================
   Utility helpers used across pages
   ============================================= */

/** Formats a number as USD currency */
function formatCurrency(amount, currency = 'USD') {
  return new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency,
    maximumFractionDigits: 0
  }).format(amount);
}

/** Formats a date string to a friendly English format */
function formatDate(dateStr) {
  if (!dateStr) return '';
  const d = new Date(dateStr);
  return d.toLocaleDateString('en-US', { day: 'numeric', month: 'long', year: 'numeric' });
}

/** Returns a Bootstrap color class for a trip status */
function statusClass(status) {
  const map = {
    draft: 'secondary',
    planning: 'primary',
    confirmed: 'success',
    completed: 'info',
    cancelled: 'danger'
  };
  return map[status] || 'secondary';
}

/** Returns a label for a trip status */
function statusLabel(status) {
  const map = {
    draft: 'Draft',
    planning: 'Planning',
    confirmed: 'Confirmed',
    completed: 'Completed',
    cancelled: 'Cancelled'
  };
  return map[status] || status;
}

/** Preference tag toggle (used on new-trip and profile pages) */
function initPrefTags() {
  document.querySelectorAll('.pref-tag').forEach(tag => {
    tag.addEventListener('click', () => {
      tag.classList.toggle('selected');
    });
  });
}
