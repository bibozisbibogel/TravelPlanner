/* =============================================
   TravelPlanner - Shared JavaScript
   ============================================= */

const DEFAULT_USER = {
  id: 'a1b2c3d4-e5f6-7890-abcd-ef1234567890',
  name: 'Cristian Stoian',
  email: 'cristian.stoian@gmail.com',
  role: 'User'
};

/**
 * Reads the current user from localStorage (persistent) or sessionStorage (session only).
 * Falls back to the default mock user so all pages show a logged-in state.
 */
function getCurrentUser() {
  try {
    const stored = localStorage.getItem('tp_user') || sessionStorage.getItem('tp_user');
    return stored ? JSON.parse(stored) : DEFAULT_USER;
  } catch {
    return DEFAULT_USER;
  }
}

/**
 * Saves a user object.
 * @param {object} user - User data to persist.
 * @param {boolean} remember - true = persist across sessions (localStorage), false = session only.
 */
function setCurrentUser(user, remember = true) {
  const data = JSON.stringify(user);
  if (remember) {
    localStorage.setItem('tp_user', data);
    sessionStorage.removeItem('tp_user');
  } else {
    sessionStorage.setItem('tp_user', data);
    localStorage.removeItem('tp_user');
  }
}

/** Clears the session from both storages and redirects to login */
function logout() {
  fetch('http://localhost:5168/api/users/logout', { method: 'POST', credentials: 'include' }).catch(() => {});
  localStorage.removeItem('tp_user');
  sessionStorage.removeItem('tp_user');
  window.location.href = 'login.html';
}

/**
 * Redirects to login.html if no authenticated user is found.
 */
function requireAuth() {
  const stored = localStorage.getItem('tp_user') || sessionStorage.getItem('tp_user');
  if (!stored) window.location.href = 'login.html';
}

/**
 * Redirects to index.html if the current user does not have the required role.
 */
function requireRole(role) {
  requireAuth();
  const user = getCurrentUser();
  if (user.role !== role) window.location.href = 'index.html';
}

/**
 * Renders the navbar user badge with avatar image (or initials) + Admin link for admins.
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

  const avatarHtml = user.avatarUrl
    ? `<img src="http://localhost:5168${user.avatarUrl}" alt="avatar"
            class="rounded-circle" style="width:32px;height:32px;object-fit:cover;"
            onerror="this.style.display='none';this.nextElementSibling.style.display='flex';">
       <span class="rounded-circle bg-success text-white d-none align-items-center justify-content-center fw-bold"
             style="width:32px;height:32px;font-size:0.8rem;">${initials}</span>`
    : `<span class="rounded-circle bg-success text-white d-inline-flex align-items-center justify-content-center fw-bold"
              style="width:32px;height:32px;font-size:0.8rem;">${initials}</span>`;

  const adminLink = user.role === 'Admin'
    ? `<li><a class="dropdown-item text-warning" href="admin.html">
         <i class="bi bi-shield-lock me-2"></i>Admin Panel
       </a></li>
       <li><hr class="dropdown-divider"></li>`
    : '';

  container.innerHTML = `
    <li class="nav-item dropdown d-flex align-items-center">
      <a class="nav-link dropdown-toggle d-flex align-items-center gap-2 user-badge"
         href="#" id="userDropdown" role="button"
         data-bs-toggle="dropdown" aria-expanded="false">
        ${avatarHtml}
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
        ${adminLink}
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

function applyTheme() {
  const saved = localStorage.getItem('tp_theme') || 'light';
  document.documentElement.setAttribute('data-theme', saved);
  updateThemeIcon(saved);
}

function toggleTheme() {
  const current = document.documentElement.getAttribute('data-theme') || 'light';
  const next = current === 'dark' ? 'light' : 'dark';
  document.documentElement.setAttribute('data-theme', next);
  localStorage.setItem('tp_theme', next);
  updateThemeIcon(next);
}

function updateThemeIcon(theme) {
  const btn = document.getElementById('themeToggle');
  if (!btn) return;
  btn.innerHTML = theme === 'dark'
    ? '<i class="bi bi-sun-fill"></i>'
    : '<i class="bi bi-moon-fill"></i>';
  btn.title = theme === 'dark' ? 'Switch to light mode' : 'Switch to dark mode';
}

function renderThemeToggle() {
  const wrap = document.getElementById('themeToggleWrap');
  if (!wrap) return;
  wrap.innerHTML = `
    <button class="theme-toggle" id="themeToggle" onclick="toggleTheme()" title="Toggle theme">
      <i class="bi bi-moon-fill"></i>
    </button>
  `;
  const current = document.documentElement.getAttribute('data-theme') || 'light';
  updateThemeIcon(current);
}

/** Initialize on every page */
document.addEventListener('DOMContentLoaded', () => {
  applyTheme();
  renderThemeToggle();
  renderNavUser();
  setActiveNavLink();
});

/* =============================================
   Utility helpers used across pages
   ============================================= */

function formatCurrency(amount, currency = 'USD') {
  return new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency,
    maximumFractionDigits: 0
  }).format(amount);
}

function formatDate(dateStr) {
  if (!dateStr) return '';
  const d = new Date(dateStr);
  return d.toLocaleDateString('en-US', { day: 'numeric', month: 'long', year: 'numeric' });
}

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

function initPrefTags() {
  document.querySelectorAll('.pref-tag').forEach(tag => {
    tag.addEventListener('click', () => tag.classList.toggle('selected'));
  });
}
